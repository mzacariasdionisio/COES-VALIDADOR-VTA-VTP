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
    /// Clase de acceso a datos de la tabla VCR_PAGORSF
    /// </summary>
    public class VcrPagorsfRepository: RepositoryBase, IVcrPagorsfRepository
    {
        public VcrPagorsfRepository(string strConn): base(strConn)
        {
        }

        VcrPagorsfHelper helper = new VcrPagorsfHelper();

        public int Save(VcrPagorsfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcprsfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcprsfpagorsf, DbType.Decimal, entity.Vcprsfpagorsf);
            dbProvider.AddInParameter(command, helper.Vcprsfusucreacion, DbType.String, entity.Vcprsfusucreacion);
            dbProvider.AddInParameter(command, helper.Vcprsffeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrPagorsfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcprsfpagorsf, DbType.Decimal, entity.Vcprsfpagorsf);
            dbProvider.AddInParameter(command, helper.Vcprsfusucreacion, DbType.String, entity.Vcprsfusucreacion);
            dbProvider.AddInParameter(command, helper.Vcprsffeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcprsfcodi, DbType.Int32, entity.Vcprsfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrPagorsfDTO GetById(int vcprsfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcprsfcodi, DbType.Int32, vcprsfcodi);
            VcrPagorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrPagorsfDTO> List()
        {
            List<VcrPagorsfDTO> entitys = new List<VcrPagorsfDTO>();
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

        public List<VcrPagorsfDTO> GetByCriteria()
        {
            List<VcrPagorsfDTO> entitys = new List<VcrPagorsfDTO>();
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

        public VcrPagorsfDTO GetByIdUnidad2020(int vcrecacodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdUnidad2020);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            VcrPagorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrPagorsfDTO();

                    int iVcprsfpagorsf = dr.GetOrdinal(this.helper.Vcprsfpagorsf);
                    if (!dr.IsDBNull(iVcprsfpagorsf)) entity.Vcprsfpagorsf = dr.GetDecimal(iVcprsfpagorsf);
                }
            }

            return entity;
        }

        public VcrPagorsfDTO GetByIdUnidad(int vcrecacodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdUnidad);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            VcrPagorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrPagorsfDTO();

                    int iVcprsfpagorsf = dr.GetOrdinal(this.helper.Vcprsfpagorsf);
                    if (!dr.IsDBNull(iVcprsfpagorsf)) entity.Vcprsfpagorsf = dr.GetDecimal(iVcprsfpagorsf);
                }
            }

            return entity;
        }

        public VcrPagorsfDTO GetByIdUnidadPorEmpresa(int vcrecacodi, int equicodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdUnidadPorEmpresa);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            VcrPagorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrPagorsfDTO();

                    int iVcprsfpagorsf = dr.GetOrdinal(this.helper.Vcprsfpagorsf);
                    if (!dr.IsDBNull(iVcprsfpagorsf)) entity.Vcprsfpagorsf = dr.GetDecimal(iVcprsfpagorsf);
                }
            }

            return entity;
        }

        public VcrPagorsfDTO GetByMigracionEquiposPorEmpresaOrigenxDestino(int vcrecacodi, int emprcodiorigen, int emprcodidestino)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByMigracionEquiposPorEmpresaOrigenxDestino);

            string query = string.Format(helper.SqlGetByMigracionEquiposPorEmpresaOrigenxDestino, vcrecacodi, emprcodiorigen, emprcodidestino);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            //dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            //dbProvider.AddInParameter(command, helper.Emprcodiorigen, DbType.Int32, emprcodiorigen);
            //dbProvider.AddInParameter(command, helper.Emprcodidestino, DbType.Int32, emprcodidestino);
            VcrPagorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrPagorsfDTO();

                    int iVcprsfpagorsf = dr.GetOrdinal(this.helper.Vcprsfpagorsf);
                    if (!dr.IsDBNull(iVcprsfpagorsf)) entity.Vcprsfpagorsf = dr.GetDecimal(iVcprsfpagorsf);
                }
            }

            return entity;
        }
    }
}

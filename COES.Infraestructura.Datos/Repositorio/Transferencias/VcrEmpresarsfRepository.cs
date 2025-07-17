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
    /// Clase de acceso a datos de la tabla VCR_EMPRESARSF
    /// </summary>
    public class VcrEmpresarsfRepository: RepositoryBase, IVcrEmpresarsfRepository
    {
        public VcrEmpresarsfRepository(string strConn): base(strConn)
        {
        }

        VcrEmpresarsfHelper helper = new VcrEmpresarsfHelper();

        public int Save(VcrEmpresarsfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcersfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Vcersfresvnosumins, DbType.Decimal, entity.Vcersfresvnosumins);
            dbProvider.AddInParameter(command, helper.Vcersftermsuperavit, DbType.Decimal, entity.Vcersftermsuperavit);
            dbProvider.AddInParameter(command, helper.Vcersfcostoportun, DbType.Decimal, entity.Vcersfcostoportun);
            dbProvider.AddInParameter(command, helper.Vcersfcompensacion, DbType.Decimal, entity.Vcersfcompensacion);
            dbProvider.AddInParameter(command, helper.Vcersfasignreserva, DbType.Decimal, entity.Vcersfasignreserva);
            dbProvider.AddInParameter(command, helper.Vcersfpagoincumpl, DbType.Decimal, entity.Vcersfpagoincumpl);
            dbProvider.AddInParameter(command, helper.Vcersfpagorsf, DbType.Decimal, entity.Vcersfpagorsf);
            dbProvider.AddInParameter(command, helper.Vcersfusucreacion, DbType.String, entity.Vcersfusucreacion);
            dbProvider.AddInParameter(command, helper.Vcersffeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrEmpresarsfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Vcersfresvnosumins, DbType.Decimal, entity.Vcersfresvnosumins);
            dbProvider.AddInParameter(command, helper.Vcersftermsuperavit, DbType.Decimal, entity.Vcersftermsuperavit);
            dbProvider.AddInParameter(command, helper.Vcersfcostoportun, DbType.Decimal, entity.Vcersfcostoportun);
            dbProvider.AddInParameter(command, helper.Vcersfcompensacion, DbType.Decimal, entity.Vcersfcompensacion);
            dbProvider.AddInParameter(command, helper.Vcersfasignreserva, DbType.Decimal, entity.Vcersfasignreserva);
            dbProvider.AddInParameter(command, helper.Vcersfpagoincumpl, DbType.Decimal, entity.Vcersfpagoincumpl);
            dbProvider.AddInParameter(command, helper.Vcersfpagorsf, DbType.Decimal, entity.Vcersfpagorsf);
            dbProvider.AddInParameter(command, helper.Vcersfusucreacion, DbType.String, entity.Vcersfusucreacion);
            dbProvider.AddInParameter(command, helper.Vcersffeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcersfcodi, DbType.Int32, entity.Vcersfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrEmpresarsfDTO GetById(int vcersfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcersfcodi, DbType.Int32, vcersfcodi);
            VcrEmpresarsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrEmpresarsfDTO> List(int vcrecacodi)
        {
            List<VcrEmpresarsfDTO> entitys = new List<VcrEmpresarsfDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrEmpresarsfDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrEmpresarsfDTO> GetByCriteria()
        {
            List<VcrEmpresarsfDTO> entitys = new List<VcrEmpresarsfDTO>();
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

        public VcrEmpresarsfDTO GetByIdTotalMes(int vcrecacodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdTotalMes);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            VcrEmpresarsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrEmpresarsfDTO();

                    int iVcersfresvnosumins = dr.GetOrdinal(this.helper.Vcersfresvnosumins);
                    if (!dr.IsDBNull(iVcersfresvnosumins)) entity.Vcersfresvnosumins = dr.GetDecimal(iVcersfresvnosumins);

                    int iVcersftermsuperavit = dr.GetOrdinal(this.helper.Vcersftermsuperavit);
                    if (!dr.IsDBNull(iVcersftermsuperavit)) entity.Vcersftermsuperavit = dr.GetDecimal(iVcersftermsuperavit);

                    int iVcersfcostoportun = dr.GetOrdinal(this.helper.Vcersfcostoportun);
                    if (!dr.IsDBNull(iVcersfcostoportun)) entity.Vcersfcostoportun = dr.GetDecimal(iVcersfcostoportun);

                    int iVcersfcompensacion = dr.GetOrdinal(this.helper.Vcersfcompensacion);
                    if (!dr.IsDBNull(iVcersfcompensacion)) entity.Vcersfcompensacion = dr.GetDecimal(iVcersfcompensacion);

                    int iVcersfasignreserva = dr.GetOrdinal(this.helper.Vcersfasignreserva);
                    if (!dr.IsDBNull(iVcersfasignreserva)) entity.Vcersfasignreserva = dr.GetDecimal(iVcersfasignreserva);

                    int iVcersfpagoincumpl = dr.GetOrdinal(this.helper.Vcersfpagoincumpl);
                    if (!dr.IsDBNull(iVcersfpagoincumpl)) entity.Vcersfpagoincumpl = dr.GetDecimal(iVcersfpagoincumpl);

                    int iVcersfpagorsf = dr.GetOrdinal(this.helper.Vcersfpagorsf);
                    if (!dr.IsDBNull(iVcersfpagorsf)) entity.Vcersfpagorsf = dr.GetDecimal(iVcersfpagorsf);
                }
            }

            return entity;
        }
    }
}

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
    /// Clase de acceso a datos de la tabla VCR_SERVICIORSF
    /// </summary>
    public class VcrServiciorsfRepository: RepositoryBase, IVcrServiciorsfRepository
    {
        public VcrServiciorsfRepository(string strConn): base(strConn)
        {
        }

        VcrServiciorsfHelper helper = new VcrServiciorsfHelper();

        public int Save(VcrServiciorsfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcsrsfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcsrsffecha, DbType.DateTime, entity.Vcsrsffecha);
            dbProvider.AddInParameter(command, helper.Vcsrsfasignreserva, DbType.Decimal, entity.Vcsrsfasignreserva);
            dbProvider.AddInParameter(command, helper.Vcsrsfcostportun, DbType.Decimal, entity.Vcsrsfcostportun);
            dbProvider.AddInParameter(command, helper.Vcsrsfcostotcomps, DbType.Decimal, entity.Vcsrsfcostotcomps);
            dbProvider.AddInParameter(command, helper.Vcsrsfresvnosumn, DbType.Decimal, entity.Vcsrsfresvnosumn);
            dbProvider.AddInParameter(command, helper.Vcsrscostotservrsf, DbType.Decimal, entity.Vcsrscostotservrsf);
            dbProvider.AddInParameter(command, helper.Vcsrsfusucreacion, DbType.String, entity.Vcsrsfusucreacion);
            dbProvider.AddInParameter(command, helper.Vcsrsffeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrServiciorsfDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcsrsffecha, DbType.DateTime, entity.Vcsrsffecha);
            dbProvider.AddInParameter(command, helper.Vcsrsfasignreserva, DbType.Decimal, entity.Vcsrsfasignreserva);
            dbProvider.AddInParameter(command, helper.Vcsrsfcostportun, DbType.Decimal, entity.Vcsrsfcostportun);
            dbProvider.AddInParameter(command, helper.Vcsrsfcostotcomps, DbType.Decimal, entity.Vcsrsfcostotcomps);
            dbProvider.AddInParameter(command, helper.Vcsrsfresvnosumn, DbType.Decimal, entity.Vcsrsfresvnosumn);
            dbProvider.AddInParameter(command, helper.Vcsrscostotservrsf, DbType.Decimal, entity.Vcsrscostotservrsf);
            dbProvider.AddInParameter(command, helper.Vcsrsfusucreacion, DbType.String, entity.Vcsrsfusucreacion);
            dbProvider.AddInParameter(command, helper.Vcsrsffeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcsrsfcodi, DbType.Int32, entity.Vcsrsfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrServiciorsfDTO GetById(int vcsrsfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcsrsfcodi, DbType.Int32, vcsrsfcodi);
            VcrServiciorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrServiciorsfDTO GetByIdValoresDia(int vcrecacodi, DateTime vcsrsffecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdValoresDia);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcsrsffecha, DbType.DateTime, vcsrsffecha);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcsrsffecha, DbType.DateTime, vcsrsffecha);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcsrsffecha, DbType.DateTime, vcsrsffecha);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcsrsffecha, DbType.DateTime, vcsrsffecha);
            VcrServiciorsfDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrServiciorsfDTO();

                    int iVcsrsfasignreserva = dr.GetOrdinal(this.helper.Vcsrsfasignreserva);
                    if (!dr.IsDBNull(iVcsrsfasignreserva)) entity.Vcsrsfasignreserva = dr.GetDecimal(iVcsrsfasignreserva);

                    int iVcsrsfcostportun = dr.GetOrdinal(this.helper.Vcsrsfcostportun);
                    if (!dr.IsDBNull(iVcsrsfcostportun)) entity.Vcsrsfcostportun = dr.GetDecimal(iVcsrsfcostportun);

                    int iVcsrsfcostotcomps = dr.GetOrdinal(this.helper.Vcsrsfcostotcomps);
                    if (!dr.IsDBNull(iVcsrsfcostotcomps)) entity.Vcsrsfcostotcomps = dr.GetDecimal(iVcsrsfcostotcomps);

                    int iVcsrsfresvnosumn = dr.GetOrdinal(this.helper.Vcsrsfresvnosumn);
                    if (!dr.IsDBNull(iVcsrsfresvnosumn)) entity.Vcsrsfresvnosumn = dr.GetDecimal(iVcsrsfresvnosumn);
                }
            }

            return entity;
        }

        public List<VcrServiciorsfDTO> List(int vcrecacodi)
        {
            List<VcrServiciorsfDTO> entitys = new List<VcrServiciorsfDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrServiciorsfDTO> GetByCriteria()
        {
            List<VcrServiciorsfDTO> entitys = new List<VcrServiciorsfDTO>();
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
    }
}

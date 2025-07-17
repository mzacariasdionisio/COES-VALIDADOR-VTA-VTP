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
    /// Clase de acceso a datos de la tabla VCR_CARGOINCUMPL
    /// </summary>
    public class VcrCargoincumplRepository: RepositoryBase, IVcrCargoincumplRepository
    {
        public VcrCargoincumplRepository(string strConn): base(strConn)
        {
        }

        VcrCargoincumplHelper helper = new VcrCargoincumplHelper();

        public int Save(VcrCargoincumplDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrcicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcrcicargoincumplmes, DbType.Decimal, entity.Vcrcicargoincumplmes);
            dbProvider.AddInParameter(command, helper.Vcrcisaldoanterior, DbType.Decimal, entity.Vcrcisaldoanterior);
            dbProvider.AddInParameter(command, helper.Vcrcicargoincumpl, DbType.Decimal, entity.Vcrcicargoincumpl);
            dbProvider.AddInParameter(command, helper.Vcrcicarginctransf, DbType.Decimal, entity.Vcrcicarginctransf);
            dbProvider.AddInParameter(command, helper.Vcrcisaldomes, DbType.Decimal, entity.Vcrcisaldomes);
            dbProvider.AddInParameter(command, helper.Pericodidest, DbType.Int32, entity.Pericodidest);
            dbProvider.AddInParameter(command, helper.Vcrciusucreacion, DbType.String, entity.Vcrciusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrcifeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrcisaldomesanterior, DbType.Decimal, entity.VcrcisaldomesAnterior);
            //ASSETEC 202012
            dbProvider.AddInParameter(command, helper.Vcrciincumplsrvrsf, DbType.Decimal, entity.Vcrciincumplsrvrsf);
            dbProvider.AddInParameter(command, helper.Vcrciincent, DbType.Decimal, entity.Vcrciincent);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrCargoincumplDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrcicarginctransf, DbType.Decimal, entity.Vcrcicarginctransf);
            dbProvider.AddInParameter(command, helper.Vcrcisaldomes, DbType.Decimal, entity.Vcrcisaldomes);
            //ASSETEC 202012
            dbProvider.AddInParameter(command, helper.Vcrciincent, DbType.Decimal, entity.Vcrciincent); 

            dbProvider.AddInParameter(command, helper.Vcrcicodi, DbType.Int32, entity.Vcrcicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrcicodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrCargoincumplDTO GetById(int vcrecacodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            VcrCargoincumplDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrCargoincumplDTO> List(int vcrecacodi)
        {
            List<VcrCargoincumplDTO> entitys = new List<VcrCargoincumplDTO>();
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

        public List<VcrCargoincumplDTO> GetByCriteria()
        {
            List<VcrCargoincumplDTO> entitys = new List<VcrCargoincumplDTO>();
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

        public List<VcrCargoincumplDTO> ListCargoIncumplGrupoCalculado(int vcrecacodi)
        {
            List<VcrCargoincumplDTO> entitys = new List<VcrCargoincumplDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCargoIncumplGrupoCalculado);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrCargoincumplDTO entity = new VcrCargoincumplDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iVcrcicargoincumpl = dr.GetOrdinal(this.helper.Vcrcicargoincumpl);
                    if (!dr.IsDBNull(iVcrcicargoincumpl)) entity.Vcrcicargoincumpl = dr.GetDecimal(iVcrcicargoincumpl);

                    int iVcrciincumplsrvrsf = dr.GetOrdinal(this.helper.Vcrciincumplsrvrsf);
                    if (!dr.IsDBNull(iVcrciincumplsrvrsf)) entity.Vcrciincumplsrvrsf = dr.GetDecimal(iVcrciincumplsrvrsf);

                    //sum(vcrmebpresencia) -> totmebpresencia
                    int iPericodidest = dr.GetOrdinal(this.helper.Pericodidest);
                    if (!dr.IsDBNull(iPericodidest)) entity.Pericodidest = Convert.ToInt32(dr.GetValue(iPericodidest));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal TotalMesServicioRSFConsiderados(int vcrecacodi)
        {
            decimal dTotalMesServicioRSFConsiderados = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalMesServicioRSFConsiderados);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iVcrciincumplsrvrsf = dr.GetOrdinal(this.helper.Vcrciincumplsrvrsf);
                    if (!dr.IsDBNull(iVcrciincumplsrvrsf)) dTotalMesServicioRSFConsiderados = dr.GetDecimal(iVcrciincumplsrvrsf);

                }
            }

            return dTotalMesServicioRSFConsiderados;
        }
    }
}

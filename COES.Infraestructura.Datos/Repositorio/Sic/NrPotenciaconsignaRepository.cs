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
    /// Clase de acceso a datos de la tabla NR_POTENCIACONSIGNA
    /// </summary>
    public class NrPotenciaconsignaRepository: RepositoryBase, INrPotenciaconsignaRepository
    {
        public NrPotenciaconsignaRepository(string strConn): base(strConn)
        {
        }

        NrPotenciaconsignaHelper helper = new NrPotenciaconsignaHelper();

        public int Save(NrPotenciaconsignaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, entity.Nrsmodcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Nrpcfecha, DbType.DateTime, entity.Nrpcfecha);
            dbProvider.AddInParameter(command, helper.Nrpceliminado, DbType.String, entity.Nrpceliminado);
            dbProvider.AddInParameter(command, helper.Nrpcusucreacion, DbType.String, entity.Nrpcusucreacion);
            dbProvider.AddInParameter(command, helper.Nrpcfeccreacion, DbType.DateTime, entity.Nrpcfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrpcusumodificacion, DbType.String, entity.Nrpcusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrpcfecmodificacion, DbType.DateTime, entity.Nrpcfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrPotenciaconsignaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, entity.Nrsmodcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Nrpcfecha, DbType.DateTime, entity.Nrpcfecha);
            dbProvider.AddInParameter(command, helper.Nrpceliminado, DbType.String, entity.Nrpceliminado);
            dbProvider.AddInParameter(command, helper.Nrpcusucreacion, DbType.String, entity.Nrpcusucreacion);
            dbProvider.AddInParameter(command, helper.Nrpcfeccreacion, DbType.DateTime, entity.Nrpcfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrpcusumodificacion, DbType.String, entity.Nrpcusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrpcfecmodificacion, DbType.DateTime, entity.Nrpcfecmodificacion);
            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, entity.Nrpccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrpccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, nrpccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public NrPotenciaconsignaDTO GetById(int nrpccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, nrpccodi);
            NrPotenciaconsignaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<NrPotenciaconsignaDTO> List()
        {
            List<NrPotenciaconsignaDTO> entitys = new List<NrPotenciaconsignaDTO>();
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

        public List<NrPotenciaconsignaDTO> GetByCriteria()
        {
            List<NrPotenciaconsignaDTO> entitys = new List<NrPotenciaconsignaDTO>();
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
        /// <summary>
        /// Graba los datos de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public int SaveNrPotenciaconsignaId(NrPotenciaconsignaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrpccodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrpccodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<NrPotenciaconsignaDTO> BuscarOperaciones(int nrsmodCodi, int grupoCodi, DateTime nrpcFechaIni, DateTime nrpcFechaFin, string estado, int nroPage, int pageSize)
        {
            List<NrPotenciaconsignaDTO> entitys = new List<NrPotenciaconsignaDTO>();
            String sql = String.Format(this.helper.ObtenerListado, nrsmodCodi, grupoCodi, nrpcFechaIni.ToString(ConstantesBase.FormatoFecha), nrpcFechaFin.ToString(ConstantesBase.FormatoFecha), estado, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrPotenciaconsignaDTO entity = new NrPotenciaconsignaDTO();

                    int iNrpccodi = dr.GetOrdinal(this.helper.Nrpccodi);
                    if (!dr.IsDBNull(iNrpccodi)) entity.Nrpccodi = Convert.ToInt32(dr.GetValue(iNrpccodi));

                    int iNrsmodcodi = dr.GetOrdinal(this.helper.Nrsmodcodi);
                    if (!dr.IsDBNull(iNrsmodcodi)) entity.Nrsmodcodi = Convert.ToInt32(dr.GetValue(iNrsmodcodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iNrpcfecha = dr.GetOrdinal(this.helper.Nrpcfecha);
                    if (!dr.IsDBNull(iNrpcfecha)) entity.Nrpcfecha = dr.GetDateTime(iNrpcfecha);

                    int iNrpceliminado = dr.GetOrdinal(this.helper.Nrpceliminado);
                    if (!dr.IsDBNull(iNrpceliminado)) entity.Nrpceliminado = dr.GetString(iNrpceliminado);

                    int iNrpcusucreacion = dr.GetOrdinal(this.helper.Nrpcusucreacion);
                    if (!dr.IsDBNull(iNrpcusucreacion)) entity.Nrpcusucreacion = dr.GetString(iNrpcusucreacion);

                    int iNrpcfeccreacion = dr.GetOrdinal(this.helper.Nrpcfeccreacion);
                    if (!dr.IsDBNull(iNrpcfeccreacion)) entity.Nrpcfeccreacion = dr.GetDateTime(iNrpcfeccreacion);

                    int iNrpcusumodificacion = dr.GetOrdinal(this.helper.Nrpcusumodificacion);
                    if (!dr.IsDBNull(iNrpcusumodificacion)) entity.Nrpcusumodificacion = dr.GetString(iNrpcusumodificacion);

                    int iNrpcfecmodificacion = dr.GetOrdinal(this.helper.Nrpcfecmodificacion);
                    if (!dr.IsDBNull(iNrpcfecmodificacion)) entity.Nrpcfecmodificacion = dr.GetDateTime(iNrpcfecmodificacion);

                    int iNrsmodnombre = dr.GetOrdinal(this.helper.Nrsmodnombre);
                    if (!dr.IsDBNull(iNrsmodnombre)) entity.Nrsmodnombre = dr.GetString(iNrsmodnombre);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int nrsmodCodi, int grupoCodi, DateTime nrpcFechaIni, DateTime nrpcFechaFin, string estado)
        {
            String sql = String.Format(this.helper.TotalRegistros, nrsmodCodi, grupoCodi, nrpcFechaIni.ToString(ConstantesBase.FormatoFecha),
                nrpcFechaFin.ToString(ConstantesBase.FormatoFecha), estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}

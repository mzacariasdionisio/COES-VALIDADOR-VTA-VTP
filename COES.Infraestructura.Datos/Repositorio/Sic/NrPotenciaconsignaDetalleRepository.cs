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
    /// Clase de acceso a datos de la tabla NR_POTENCIACONSIGNA_DETALLE
    /// </summary>
    public class NrPotenciaconsignaDetalleRepository: RepositoryBase, INrPotenciaconsignaDetalleRepository
    {
        public NrPotenciaconsignaDetalleRepository(string strConn): base(strConn)
        {
        }

        NrPotenciaconsignaDetalleHelper helper = new NrPotenciaconsignaDetalleHelper();

        public int Save(NrPotenciaconsignaDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrpcdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, entity.Nrpccodi);
            dbProvider.AddInParameter(command, helper.Nrpcdfecha, DbType.DateTime, entity.Nrpcdfecha);
            dbProvider.AddInParameter(command, helper.Nrpcdmw, DbType.Decimal, entity.Nrpcdmw);
            dbProvider.AddInParameter(command, helper.Nrpcdmaximageneracion, DbType.String, entity.Nrpcdmaximageneracion);
            dbProvider.AddInParameter(command, helper.Nrpcdusucreacion, DbType.String, entity.Nrpcdusucreacion);
            dbProvider.AddInParameter(command, helper.Nrpcdfeccreacion, DbType.DateTime, entity.Nrpcdfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrpcdusumodificacion, DbType.String, entity.Nrpcdusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrpcdfecmodificacion, DbType.DateTime, entity.Nrpcdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrPotenciaconsignaDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, entity.Nrpccodi);
            dbProvider.AddInParameter(command, helper.Nrpcdfecha, DbType.DateTime, entity.Nrpcdfecha);
            dbProvider.AddInParameter(command, helper.Nrpcdmw, DbType.Decimal, entity.Nrpcdmw);
            dbProvider.AddInParameter(command, helper.Nrpcdmaximageneracion, DbType.String, entity.Nrpcdmaximageneracion);
            dbProvider.AddInParameter(command, helper.Nrpcdusucreacion, DbType.String, entity.Nrpcdusucreacion);
            dbProvider.AddInParameter(command, helper.Nrpcdfeccreacion, DbType.DateTime, entity.Nrpcdfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrpcdusumodificacion, DbType.String, entity.Nrpcdusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrpcdfecmodificacion, DbType.DateTime, entity.Nrpcdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Nrpcdcodi, DbType.Int32, entity.Nrpcdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrpcdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrpcdcodi, DbType.Int32, nrpcdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteTotal(int nrpccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteTotal);

            dbProvider.AddInParameter(command, helper.Nrpccodi, DbType.Int32, nrpccodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public NrPotenciaconsignaDetalleDTO GetById(int nrpcdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrpcdcodi, DbType.Int32, nrpcdcodi);
            NrPotenciaconsignaDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<NrPotenciaconsignaDetalleDTO> List()
        {
            List<NrPotenciaconsignaDetalleDTO> entitys = new List<NrPotenciaconsignaDetalleDTO>();
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

        public List<NrPotenciaconsignaDetalleDTO> GetByCriteria()
        {
            List<NrPotenciaconsignaDetalleDTO> entitys = new List<NrPotenciaconsignaDetalleDTO>();
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
        /// Graba los datos de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public int SaveNrPotenciaconsignaDetalleId(NrPotenciaconsignaDetalleDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrpcdcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrpcdcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<NrPotenciaconsignaDetalleDTO> BuscarOperaciones(int nrpcCodi,DateTime nrpcdFecha,DateTime nrpcdFecCreacion, int nroPage, int pageSize)
        {
            List<NrPotenciaconsignaDetalleDTO> entitys = new List<NrPotenciaconsignaDetalleDTO>();
            String sql = String.Format(this.helper.ObtenerListado, nrpcCodi,nrpcdFecha.ToString(ConstantesBase.FormatoFecha),nrpcdFecCreacion.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrPotenciaconsignaDetalleDTO entity = new NrPotenciaconsignaDetalleDTO();

                    int iNrpcdcodi = dr.GetOrdinal(this.helper.Nrpcdcodi);
                    if (!dr.IsDBNull(iNrpcdcodi)) entity.Nrpcdcodi = Convert.ToInt32(dr.GetValue(iNrpcdcodi));

                    int iNrpccodi = dr.GetOrdinal(this.helper.Nrpccodi);
                    if (!dr.IsDBNull(iNrpccodi)) entity.Nrpccodi = Convert.ToInt32(dr.GetValue(iNrpccodi));

                    int iNrpcdfecha = dr.GetOrdinal(this.helper.Nrpcdfecha);
                    if (!dr.IsDBNull(iNrpcdfecha)) entity.Nrpcdfecha = dr.GetDateTime(iNrpcdfecha);

                    int iNrpcdmw = dr.GetOrdinal(this.helper.Nrpcdmw);
                    if (!dr.IsDBNull(iNrpcdmw)) entity.Nrpcdmw = dr.GetDecimal(iNrpcdmw);

                    int iNrpcdmaximageneracion = dr.GetOrdinal(this.helper.Nrpcdmaximageneracion);
                    if (!dr.IsDBNull(iNrpcdmaximageneracion)) entity.Nrpcdmaximageneracion = dr.GetString(iNrpcdmaximageneracion);

                    int iNrpcdusucreacion = dr.GetOrdinal(this.helper.Nrpcdusucreacion);
                    if (!dr.IsDBNull(iNrpcdusucreacion)) entity.Nrpcdusucreacion = dr.GetString(iNrpcdusucreacion);

                    int iNrpcdfeccreacion = dr.GetOrdinal(this.helper.Nrpcdfeccreacion);
                    if (!dr.IsDBNull(iNrpcdfeccreacion)) entity.Nrpcdfeccreacion = dr.GetDateTime(iNrpcdfeccreacion);

                    int iNrpcdusumodificacion = dr.GetOrdinal(this.helper.Nrpcdusumodificacion);
                    if (!dr.IsDBNull(iNrpcdusumodificacion)) entity.Nrpcdusumodificacion = dr.GetString(iNrpcdusumodificacion);

                    int iNrpcdfecmodificacion = dr.GetOrdinal(this.helper.Nrpcdfecmodificacion);
                    if (!dr.IsDBNull(iNrpcdfecmodificacion)) entity.Nrpcdfecmodificacion = dr.GetDateTime(iNrpcdfecmodificacion);

                    int iNrpceliminado = dr.GetOrdinal(this.helper.Nrpceliminado);
                    if (!dr.IsDBNull(iNrpceliminado)) entity.Nrpceliminado = dr.GetString(iNrpceliminado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int nrpcCodi,DateTime nrpcdFecha,DateTime nrpcdFecCreacion)
        {
            String sql = String.Format(this.helper.TotalRegistros, nrpcCodi,nrpcdFecha.ToString(ConstantesBase.FormatoFecha),nrpcdFecCreacion.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}


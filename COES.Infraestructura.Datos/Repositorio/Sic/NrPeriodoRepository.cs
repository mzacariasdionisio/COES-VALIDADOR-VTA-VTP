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
    /// Clase de acceso a datos de la tabla NR_PERIODO
    /// </summary>
    public class NrPeriodoRepository: RepositoryBase, INrPeriodoRepository
    {
        public NrPeriodoRepository(string strConn): base(strConn)
        {
        }

        NrPeriodoHelper helper = new NrPeriodoHelper();

        public int Save(NrPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrpermes, DbType.DateTime, entity.Nrpermes);
            dbProvider.AddInParameter(command, helper.Nrpereliminado, DbType.String, entity.Nrpereliminado);
            dbProvider.AddInParameter(command, helper.Nrperusucreacion, DbType.String, entity.Nrperusucreacion);
            dbProvider.AddInParameter(command, helper.Nrperfeccreacion, DbType.DateTime, entity.Nrperfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrperusumodificacion, DbType.String, entity.Nrperusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrperfecmodificacion, DbType.DateTime, entity.Nrperfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrpermes, DbType.DateTime, entity.Nrpermes);
            dbProvider.AddInParameter(command, helper.Nrpereliminado, DbType.String, entity.Nrpereliminado);
            dbProvider.AddInParameter(command, helper.Nrperusucreacion, DbType.String, entity.Nrperusucreacion);
            dbProvider.AddInParameter(command, helper.Nrperfeccreacion, DbType.DateTime, entity.Nrperfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrperusumodificacion, DbType.String, entity.Nrperusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrperfecmodificacion, DbType.DateTime, entity.Nrperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, entity.Nrpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, nrpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public NrPeriodoDTO GetById(int nrpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, nrpercodi);
            NrPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<NrPeriodoDTO> List()
        {
            List<NrPeriodoDTO> entitys = new List<NrPeriodoDTO>();
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

        public List<NrPeriodoDTO> GetByCriteria()
        {
            List<NrPeriodoDTO> entitys = new List<NrPeriodoDTO>();
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
        /// Graba los datos de la tabla NR_PERIODO
        /// </summary>
        public int SaveNrPeriodoId(NrPeriodoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrpercodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrpercodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<NrPeriodoDTO> BuscarOperaciones(string estado, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<NrPeriodoDTO> entitys = new List<NrPeriodoDTO>();
            String sql = String.Format(this.helper.ObtenerListado, 
                estado,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrPeriodoDTO entity = new NrPeriodoDTO();

                    int iNrpercodi = dr.GetOrdinal(this.helper.Nrpercodi);
                    if (!dr.IsDBNull(iNrpercodi)) entity.Nrpercodi = Convert.ToInt32(dr.GetValue(iNrpercodi));

                    int iNrpermes = dr.GetOrdinal(this.helper.Nrpermes);
                    if (!dr.IsDBNull(iNrpermes)) entity.Nrpermes = dr.GetDateTime(iNrpermes);

                    int iNrpereliminado = dr.GetOrdinal(this.helper.Nrpereliminado);
                    if (!dr.IsDBNull(iNrpereliminado)) entity.Nrpereliminado = dr.GetString(iNrpereliminado);

                    int iNrperusucreacion = dr.GetOrdinal(this.helper.Nrperusucreacion);
                    if (!dr.IsDBNull(iNrperusucreacion)) entity.Nrperusucreacion = dr.GetString(iNrperusucreacion);

                    int iNrperfeccreacion = dr.GetOrdinal(this.helper.Nrperfeccreacion);
                    if (!dr.IsDBNull(iNrperfeccreacion)) entity.Nrperfeccreacion = dr.GetDateTime(iNrperfeccreacion);

                    int iNrperusumodificacion = dr.GetOrdinal(this.helper.Nrperusumodificacion);
                    if (!dr.IsDBNull(iNrperusumodificacion)) entity.Nrperusumodificacion = dr.GetString(iNrperusumodificacion);

                    int iNrperfecmodificacion = dr.GetOrdinal(this.helper.Nrperfecmodificacion);
                    if (!dr.IsDBNull(iNrperfecmodificacion)) entity.Nrperfecmodificacion = dr.GetDateTime(iNrperfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string estado, DateTime fechaInicio, DateTime fechaFinal)
        {
            String sql = String.Format(this.helper.TotalRegistros, 
                estado,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha));
                          

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}

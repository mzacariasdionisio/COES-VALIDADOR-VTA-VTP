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
    /// Clase de acceso a datos de la tabla SI_PARAMETRO_VALOR
    /// </summary>
    public class SiParametroValorRepository: RepositoryBase, ISiParametroValorRepository
    {
        public SiParametroValorRepository(string strConn): base(strConn)
        {
        }

        SiParametroValorHelper helper = new SiParametroValorHelper();

        public int Save(SiParametroValorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Siparvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Siparcodi, DbType.Int32, entity.Siparcodi);
            dbProvider.AddInParameter(command, helper.Siparvfechainicial, DbType.DateTime, entity.Siparvfechainicial);
            dbProvider.AddInParameter(command, helper.Siparvfechafinal, DbType.DateTime, entity.Siparvfechafinal);
            dbProvider.AddInParameter(command, helper.Siparvvalor, DbType.Decimal, entity.Siparvvalor);
            dbProvider.AddInParameter(command, helper.Siparvnota, DbType.String, entity.Siparvnota);
            dbProvider.AddInParameter(command, helper.Siparveliminado, DbType.String, entity.Siparveliminado);
            dbProvider.AddInParameter(command, helper.Siparvusucreacion, DbType.String, entity.Siparvusucreacion);
            dbProvider.AddInParameter(command, helper.Siparvfeccreacion, DbType.DateTime, entity.Siparvfeccreacion);
            dbProvider.AddInParameter(command, helper.Siparvusumodificacion, DbType.String, entity.Siparvusumodificacion);
            dbProvider.AddInParameter(command, helper.Siparvfecmodificacion, DbType.DateTime, entity.Siparvfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiParametroValorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Siparcodi, DbType.Int32, entity.Siparcodi);
            dbProvider.AddInParameter(command, helper.Siparvfechainicial, DbType.DateTime, entity.Siparvfechainicial);
            dbProvider.AddInParameter(command, helper.Siparvfechafinal, DbType.DateTime, entity.Siparvfechafinal);
            dbProvider.AddInParameter(command, helper.Siparvvalor, DbType.Decimal, entity.Siparvvalor);
            dbProvider.AddInParameter(command, helper.Siparvnota, DbType.String, entity.Siparvnota);
            dbProvider.AddInParameter(command, helper.Siparveliminado, DbType.String, entity.Siparveliminado);
            dbProvider.AddInParameter(command, helper.Siparvusucreacion, DbType.String, entity.Siparvusucreacion);
            dbProvider.AddInParameter(command, helper.Siparvfeccreacion, DbType.DateTime, entity.Siparvfeccreacion);
            dbProvider.AddInParameter(command, helper.Siparvusumodificacion, DbType.String, entity.Siparvusumodificacion);
            dbProvider.AddInParameter(command, helper.Siparvfecmodificacion, DbType.DateTime, entity.Siparvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Siparvcodi, DbType.Int32, entity.Siparvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int siparvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Siparvcodi, DbType.Int32, siparvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiParametroValorDTO GetById(int siparvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Siparvcodi, DbType.Int32, siparvcodi);
            SiParametroValorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiParametroValorDTO> List()
        {
            List<SiParametroValorDTO> entitys = new List<SiParametroValorDTO>();
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

        public List<SiParametroValorDTO> GetByCriteria()
        {
            List<SiParametroValorDTO> entitys = new List<SiParametroValorDTO>();
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
        /// Graba los datos de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public int SaveSiParametroValorId(SiParametroValorDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Siparvcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Siparvcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<SiParametroValorDTO> BuscarOperaciones(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, int nroPage, int pageSize, string estado)
        {
            List<SiParametroValorDTO> entitys = new List<SiParametroValorDTO>();
            String sql = String.Format(this.helper.ObtenerListado, siparCodi,siparvFechaInicial.ToString(ConstantesBase.FormatoFecha),siparvFechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiParametroValorDTO entity = new SiParametroValorDTO();

                    int iSiparvcodi = dr.GetOrdinal(this.helper.Siparvcodi);
                    if (!dr.IsDBNull(iSiparvcodi)) entity.Siparvcodi = Convert.ToInt32(dr.GetValue(iSiparvcodi));

                    int iSiparcodi = dr.GetOrdinal(this.helper.Siparcodi);
                    if (!dr.IsDBNull(iSiparcodi)) entity.Siparcodi = Convert.ToInt32(dr.GetValue(iSiparcodi));

                    int iSiparvfechainicial = dr.GetOrdinal(this.helper.Siparvfechainicial);
                    if (!dr.IsDBNull(iSiparvfechainicial)) entity.Siparvfechainicial = dr.GetDateTime(iSiparvfechainicial);

                    int iSiparvfechafinal = dr.GetOrdinal(this.helper.Siparvfechafinal);
                    if (!dr.IsDBNull(iSiparvfechafinal)) entity.Siparvfechafinal = dr.GetDateTime(iSiparvfechafinal);

                    int iSiparvvalor = dr.GetOrdinal(this.helper.Siparvvalor);
                    if (!dr.IsDBNull(iSiparvvalor)) entity.Siparvvalor = dr.GetDecimal(iSiparvvalor);

                    int iSiparvnota = dr.GetOrdinal(this.helper.Siparvnota);
                    if (!dr.IsDBNull(iSiparvnota)) entity.Siparvnota = dr.GetString(iSiparvnota);

                    int iSiparveliminado = dr.GetOrdinal(this.helper.Siparveliminado);
                    if (!dr.IsDBNull(iSiparveliminado)) entity.Siparveliminado = dr.GetString(iSiparveliminado);

                    int iSiparvusucreacion = dr.GetOrdinal(this.helper.Siparvusucreacion);
                    if (!dr.IsDBNull(iSiparvusucreacion)) entity.Siparvusucreacion = dr.GetString(iSiparvusucreacion);

                    int iSiparvfeccreacion = dr.GetOrdinal(this.helper.Siparvfeccreacion);
                    if (!dr.IsDBNull(iSiparvfeccreacion)) entity.Siparvfeccreacion = dr.GetDateTime(iSiparvfeccreacion);

                    int iSiparvusumodificacion = dr.GetOrdinal(this.helper.Siparvusumodificacion);
                    if (!dr.IsDBNull(iSiparvusumodificacion)) entity.Siparvusumodificacion = dr.GetString(iSiparvusumodificacion);

                    int iSiparvfecmodificacion = dr.GetOrdinal(this.helper.Siparvfecmodificacion);
                    if (!dr.IsDBNull(iSiparvfecmodificacion)) entity.Siparvfecmodificacion = dr.GetDateTime(iSiparvfecmodificacion);

                    int iSiparabrev = dr.GetOrdinal(this.helper.Siparabrev);
                    if (!dr.IsDBNull(iSiparabrev)) entity.Siparabrev = dr.GetString(iSiparabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, string estado)
        {
            String sql = String.Format(this.helper.TotalRegistros, siparCodi, siparvFechaInicial.ToString(ConstantesBase.FormatoFecha), siparvFechaFinal.ToString(ConstantesBase.FormatoFecha), estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<SiParametroValorDTO> ListByIdParametro(int siparCodi)
        {
            List<SiParametroValorDTO> entitys = new List<SiParametroValorDTO>();
            string sql = string.Format(helper.SqlListByIdParametro, siparCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiParametroValorDTO> ListByIdParametroAndFechaInicial(int siparCodi, DateTime fechaInicial)
        {
            List<SiParametroValorDTO> entitys = new List<SiParametroValorDTO>();
            string sql = string.Format(helper.SqlListByIdParametroAndFechaInicial, siparCodi, fechaInicial.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public decimal ObtenerValorParametro(int parametro, DateTime fecha)
        {
            string query = string.Format(helper.SqlObtenerValorParametro, parametro, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            decimal valor = 0;
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                valor = Convert.ToDecimal(result);
            }

            return valor;        
        }
    }
}

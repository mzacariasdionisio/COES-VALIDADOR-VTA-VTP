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
    /// Clase de acceso a datos de la tabla NR_SOBRECOSTO
    /// </summary>
    public class NrSobrecostoRepository: RepositoryBase, INrSobrecostoRepository
    {
        public NrSobrecostoRepository(string strConn): base(strConn)
        {
        }

        NrSobrecostoHelper helper = new NrSobrecostoHelper();

        public int Save(NrSobrecostoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrsccodi, DbType.Int32, id);
            //dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Nrscfecha, DbType.DateTime, entity.Nrscfecha);
            dbProvider.AddInParameter(command, helper.Nrsccodespacho0, DbType.Decimal, entity.Nrsccodespacho0);
            dbProvider.AddInParameter(command, helper.Nrsccodespacho1, DbType.Decimal, entity.Nrsccodespacho1);
            dbProvider.AddInParameter(command, helper.Nrscsobrecosto, DbType.Decimal, entity.Nrscsobrecosto);
            dbProvider.AddInParameter(command, helper.Nrscnota, DbType.String, entity.Nrscnota);            
            dbProvider.AddInParameter(command, helper.Nrsceliminado, DbType.String, entity.Nrsceliminado);
            dbProvider.AddInParameter(command, helper.Nrscpadre, DbType.Int32, entity.Nrscpadre);
            dbProvider.AddInParameter(command, helper.Nrscusucreacion, DbType.String, entity.Nrscusucreacion);
            dbProvider.AddInParameter(command, helper.Nrscfeccreacion, DbType.DateTime, entity.Nrscfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrscusumodificacion, DbType.String, entity.Nrscusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrscfecmodificacion, DbType.DateTime, entity.Nrscfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrSobrecostoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Nrscfecha, DbType.DateTime, entity.Nrscfecha);
            dbProvider.AddInParameter(command, helper.Nrsccodespacho0, DbType.Decimal, entity.Nrsccodespacho0);
            dbProvider.AddInParameter(command, helper.Nrsccodespacho1, DbType.Decimal, entity.Nrsccodespacho1);
            dbProvider.AddInParameter(command, helper.Nrscsobrecosto, DbType.Decimal, entity.Nrscsobrecosto);
            dbProvider.AddInParameter(command, helper.Nrscnota, DbType.String, entity.Nrscnota);            
            dbProvider.AddInParameter(command, helper.Nrsceliminado, DbType.String, entity.Nrsceliminado);
            dbProvider.AddInParameter(command, helper.Nrscpadre, DbType.Int32, entity.Nrscpadre);
            dbProvider.AddInParameter(command, helper.Nrscusucreacion, DbType.String, entity.Nrscusucreacion);
            dbProvider.AddInParameter(command, helper.Nrscfeccreacion, DbType.DateTime, entity.Nrscfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrscusumodificacion, DbType.String, entity.Nrscusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrscfecmodificacion, DbType.DateTime, entity.Nrscfecmodificacion);
            dbProvider.AddInParameter(command, helper.Nrsccodi, DbType.Int32, entity.Nrsccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrsccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrsccodi, DbType.Int32, nrsccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public NrSobrecostoDTO GetById(int nrsccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrsccodi, DbType.Int32, nrsccodi);
            NrSobrecostoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<NrSobrecostoDTO> List()
        {
            List<NrSobrecostoDTO> entitys = new List<NrSobrecostoDTO>();
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

        public List<NrSobrecostoDTO> GetByCriteria()
        {
            List<NrSobrecostoDTO> entitys = new List<NrSobrecostoDTO>();
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
        /// Graba los datos de la tabla NR_SOBRECOSTO
        /// </summary>
        public int SaveNrSobrecostoId(NrSobrecostoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrsccodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrsccodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //public List<NrSobrecostoDTO> BuscarOperaciones(int grupoCodi, DateTime nrscFechaIni, DateTime nrscFechaFin, string estado, int nroPage, int pageSize)
        public List<NrSobrecostoDTO> BuscarOperaciones(DateTime nrscFechaIni, DateTime nrscFechaFin, string estado, int nroPage, int pageSize)
        {
            List<NrSobrecostoDTO> entitys = new List<NrSobrecostoDTO>();
            //String sql = String.Format(this.helper.ObtenerListado, grupoCodi, nrscFechaIni.ToString(ConstantesBase.FormatoFecha), nrscFechaFin.ToString(ConstantesBase.FormatoFecha),estado, nroPage, pageSize);
            String sql = String.Format(this.helper.ObtenerListado, nrscFechaIni.ToString(ConstantesBase.FormatoFecha), nrscFechaFin.ToString(ConstantesBase.FormatoFecha), estado, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrSobrecostoDTO entity = new NrSobrecostoDTO();

                    int iNrsccodi = dr.GetOrdinal(this.helper.Nrsccodi);
                    if (!dr.IsDBNull(iNrsccodi)) entity.Nrsccodi = Convert.ToInt32(dr.GetValue(iNrsccodi));

                    /*int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));*/

                    int iNrscfecha = dr.GetOrdinal(this.helper.Nrscfecha);
                    if (!dr.IsDBNull(iNrscfecha)) entity.Nrscfecha = dr.GetDateTime(iNrscfecha);

                    int iNrsccodespacho0 = dr.GetOrdinal(this.helper.Nrsccodespacho0);
                    if (!dr.IsDBNull(iNrsccodespacho0)) entity.Nrsccodespacho0 = dr.GetDecimal(iNrsccodespacho0);

                    int iNrsccodespacho1 = dr.GetOrdinal(this.helper.Nrsccodespacho1);
                    if (!dr.IsDBNull(iNrsccodespacho1)) entity.Nrsccodespacho1 = dr.GetDecimal(iNrsccodespacho1);

                    int iNrscsobrecosto = dr.GetOrdinal(this.helper.Nrscsobrecosto);
                    if (!dr.IsDBNull(iNrscsobrecosto)) entity.Nrscsobrecosto = dr.GetDecimal(iNrscsobrecosto);

                    int iNrscnota = dr.GetOrdinal(this.helper.Nrscnota);
                    if (!dr.IsDBNull(iNrscnota)) entity.Nrscnota = dr.GetString(iNrscnota);

                    int iNrsceliminado = dr.GetOrdinal(this.helper.Nrsceliminado);
                    if (!dr.IsDBNull(iNrsceliminado)) entity.Nrsceliminado = dr.GetString(iNrsceliminado);

                    int iNrscpadre = dr.GetOrdinal(this.helper.Nrscpadre);
                    if (!dr.IsDBNull(iNrscpadre)) entity.Nrscpadre = Convert.ToInt32(dr.GetValue(iNrscpadre));

                    int iNrscusucreacion = dr.GetOrdinal(this.helper.Nrscusucreacion);
                    if (!dr.IsDBNull(iNrscusucreacion)) entity.Nrscusucreacion = dr.GetString(iNrscusucreacion);

                    int iNrscfeccreacion = dr.GetOrdinal(this.helper.Nrscfeccreacion);
                    if (!dr.IsDBNull(iNrscfeccreacion)) entity.Nrscfeccreacion = dr.GetDateTime(iNrscfeccreacion);

                    int iNrscusumodificacion = dr.GetOrdinal(this.helper.Nrscusumodificacion);
                    if (!dr.IsDBNull(iNrscusumodificacion)) entity.Nrscusumodificacion = dr.GetString(iNrscusumodificacion);

                    int iNrscfecmodificacion = dr.GetOrdinal(this.helper.Nrscfecmodificacion);
                    if (!dr.IsDBNull(iNrscfecmodificacion)) entity.Nrscfecmodificacion = dr.GetDateTime(iNrscfecmodificacion);

                    //int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    //if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //public int ObtenerNroFilas(int grupoCodi, DateTime nrscFechaIni, DateTime nrscFechaFin, string estado)
        public int ObtenerNroFilas(DateTime nrscFechaIni, DateTime nrscFechaFin, string estado)
        {
            //String sql = String.Format(this.helper.TotalRegistros, grupoCodi, nrscFechaIni.ToString(ConstantesBase.FormatoFecha), nrscFechaFin.ToString(ConstantesBase.FormatoFecha), estado);
            String sql = String.Format(this.helper.TotalRegistros, nrscFechaIni.ToString(ConstantesBase.FormatoFecha), nrscFechaFin.ToString(ConstantesBase.FormatoFecha), estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}

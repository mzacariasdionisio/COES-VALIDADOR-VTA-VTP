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
    /// Clase de acceso a datos de la tabla NR_PERIODO_RESUMEN
    /// </summary>
    public class NrPeriodoResumenRepository: RepositoryBase, INrPeriodoResumenRepository
    {
        public NrPeriodoResumenRepository(string strConn): base(strConn)
        {
        }

        NrPeriodoResumenHelper helper = new NrPeriodoResumenHelper();

        public int Save(NrPeriodoResumenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrperrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, entity.Nrpercodi);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, entity.Nrcptcodi);
            dbProvider.AddInParameter(command, helper.Nrperrnumobservacion, DbType.Int32, entity.Nrperrnumobservacion);
            dbProvider.AddInParameter(command, helper.Nrperrobservacion, DbType.String, entity.Nrperrobservacion);
            dbProvider.AddInParameter(command, helper.Nrperreliminado, DbType.String, entity.Nrperreliminado);
            dbProvider.AddInParameter(command, helper.Nrperrusucreacion, DbType.String, entity.Nrperrusucreacion);
            dbProvider.AddInParameter(command, helper.Nrperrfeccreacion, DbType.DateTime, entity.Nrperrfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrperrusumodificacion, DbType.String, entity.Nrperrusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrperrfecmodificacion, DbType.DateTime, entity.Nrperrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrPeriodoResumenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, entity.Nrpercodi);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, entity.Nrcptcodi);
            dbProvider.AddInParameter(command, helper.Nrperrnumobservacion, DbType.Int32, entity.Nrperrnumobservacion);
            dbProvider.AddInParameter(command, helper.Nrperrobservacion, DbType.String, entity.Nrperrobservacion);
            dbProvider.AddInParameter(command, helper.Nrperreliminado, DbType.String, entity.Nrperreliminado);
            dbProvider.AddInParameter(command, helper.Nrperrusucreacion, DbType.String, entity.Nrperrusucreacion);
            dbProvider.AddInParameter(command, helper.Nrperrfeccreacion, DbType.DateTime, entity.Nrperrfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrperrusumodificacion, DbType.String, entity.Nrperrusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrperrfecmodificacion, DbType.DateTime, entity.Nrperrfecmodificacion);
            dbProvider.AddInParameter(command, helper.Nrperrcodi, DbType.Int32, entity.Nrperrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrperrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrperrcodi, DbType.Int32, nrperrcodi);

            dbProvider.ExecuteNonQuery(command);
        }



        public NrPeriodoResumenDTO GetById(int nrperrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrperrcodi, DbType.Int32, nrperrcodi);
            NrPeriodoResumenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public NrPeriodoResumenDTO GetById(int nrpercodi, int nrcptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPeriodoConcepto);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, nrpercodi);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, nrcptcodi);


            NrPeriodoResumenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<NrPeriodoResumenDTO> List()
        {
            List<NrPeriodoResumenDTO> entitys = new List<NrPeriodoResumenDTO>();
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

        public List<NrPeriodoResumenDTO> List(int idNrsmodCodi,int idNrperCodi)
        {
            List<NrPeriodoResumenDTO> entitys = new List<NrPeriodoResumenDTO>();

            String sql = String.Format(this.helper.SqlListSubModuloPeriodo, idNrsmodCodi, idNrperCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrPeriodoResumenDTO entity = new NrPeriodoResumenDTO();
                    
                    entity.Nrsmodcodi = idNrsmodCodi;

                    int iNrcptcodi = dr.GetOrdinal(this.helper.Nrcptcodi);
                    if (!dr.IsDBNull(iNrcptcodi)) entity.Nrcptcodi = Convert.ToInt32(dr.GetValue(iNrcptcodi));


                    int iNrcptabrev = dr.GetOrdinal(this.helper.Nrcptabrev);
                    if (!dr.IsDBNull(iNrcptabrev)) entity.Nrcptabrev = dr.GetString(iNrcptabrev);

                    int iNrcptdescripcion = dr.GetOrdinal(this.helper.Nrcptdescripcion);
                    if (!dr.IsDBNull(iNrcptdescripcion)) entity.Nrcptdescripcion = dr.GetString(iNrcptdescripcion);


                    int iPendiente = dr.GetOrdinal(this.helper.Pendiente);
                    if (!dr.IsDBNull(iPendiente)) entity.Pendiente = Convert.ToInt32(dr.GetValue(iPendiente));


                    int iObservaciones = dr.GetOrdinal(this.helper.Observaciones);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToInt32(dr.GetValue(iObservaciones));


                    int iTerminado = dr.GetOrdinal(this.helper.Terminado);
                    if (!dr.IsDBNull(iTerminado)) entity.Terminado = Convert.ToInt32(dr.GetValue(iTerminado));

                    int iUsuModificacion = dr.GetOrdinal(this.helper.Nrperrusumodificacion);
                    if (!dr.IsDBNull(iUsuModificacion)) entity.Nrperrusumodificacion = dr.GetString(iUsuModificacion);

                    int iFecModificacion = dr.GetOrdinal(this.helper.Nrperrfecmodificacion);
                    if (!dr.IsDBNull(iFecModificacion)) entity.Nrperrfecmodificacion = dr.GetDateTime(iFecModificacion);
                   


                    entitys.Add(entity);
                }
            }

            return entitys;

        }


        public List<NrPeriodoResumenDTO> GetByCriteria()
        {
            List<NrPeriodoResumenDTO> entitys = new List<NrPeriodoResumenDTO>();
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
        /// Graba los datos de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public int SaveNrPeriodoResumenId(NrPeriodoResumenDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrperrcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrperrcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<NrPeriodoResumenDTO> BuscarOperaciones(int nrsmodCodi, string estado, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<NrPeriodoResumenDTO> entitys = new List<NrPeriodoResumenDTO>();
            String sql = String.Format(this.helper.ObtenerListado, nrsmodCodi, estado, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {                
                while (dr.Read())
                {
                    NrPeriodoResumenDTO entity = new NrPeriodoResumenDTO();


                    int iNrsmodcodi = dr.GetOrdinal(this.helper.Nrsmodcodi);
                    if (!dr.IsDBNull(iNrsmodcodi)) entity.Nrsmodcodi = Convert.ToInt32(dr.GetValue(iNrsmodcodi));

                         int iNrsmodnombre = dr.GetOrdinal(this.helper.Nrsmodnombre);
                    if (!dr.IsDBNull(iNrsmodnombre)) entity.Nrsmodnombre = dr.GetString(iNrsmodnombre);

                    int iNrpercodi = dr.GetOrdinal(this.helper.Nrpercodi);
                    if (!dr.IsDBNull(iNrpercodi)) entity.Nrpercodi = Convert.ToInt32(dr.GetValue(iNrpercodi));

                    
                    int iNrpermes = dr.GetOrdinal(this.helper.Nrpermes);
                    if (!dr.IsDBNull(iNrpermes)) entity.Nrpermes = dr.GetDateTime(iNrpermes);

                         int iPendiente = dr.GetOrdinal(this.helper.Pendiente);
                    if (!dr.IsDBNull(iPendiente)) entity.Pendiente = Convert.ToInt32(dr.GetValue(iPendiente));


                         int iObservaciones = dr.GetOrdinal(this.helper.Observaciones);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToInt32(dr.GetValue(iObservaciones));


                     int iTerminado = dr.GetOrdinal(this.helper.Terminado);
                    if (!dr.IsDBNull(iTerminado)) entity.Terminado = Convert.ToInt32(dr.GetValue(iTerminado));

                    int iProceso = dr.GetOrdinal(this.helper.Proceso);
                    if (!dr.IsDBNull(iProceso)) entity.Proceso = Convert.ToInt32(dr.GetValue(iProceso));
                    
                    int iModificacion = dr.GetOrdinal(this.helper.Nrperrfecmodificacion);
                    if (!dr.IsDBNull(iModificacion)) entity.Nrperrfecmodificacion = dr.GetDateTime(iModificacion);
                   
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int nrsmodCodi, string estado, DateTime fechaInicio, DateTime fechaFinal)
        {
            String sql = String.Format(this.helper.TotalRegistros, nrsmodCodi, estado, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));
            int registros=0;

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    registros++;
                }
            }
                        
            return registros;
        }
    }
}

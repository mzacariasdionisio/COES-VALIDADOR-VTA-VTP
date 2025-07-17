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
    /// Clase de acceso a datos de la tabla EVE_RSFDETALLE
    /// </summary>
    public class EveRsfdetalleRepository: RepositoryBase, IEveRsfdetalleRepository
    {
        public EveRsfdetalleRepository(string strConn): base(strConn)
        {
        }

        EveRsfdetalleHelper helper = new EveRsfdetalleHelper();

        public int Save(EveRsfdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rsfdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, entity.Rsfhorcodi);
            dbProvider.AddInParameter(command, helper.Rsfdetvalman, DbType.Decimal, entity.Rsfdetvalman);
            dbProvider.AddInParameter(command, helper.Rsfdetvalaut, DbType.Decimal, entity.Rsfdetvalaut);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rsfdetindope, DbType.String, entity.Rsfdetindope);
            dbProvider.AddInParameter(command, helper.Rsfdetsub, DbType.Decimal, entity.Rsfdetsub);
            dbProvider.AddInParameter(command, helper.Rsfdetbaj, DbType.Decimal, entity.Rsfdetbaj);
            dbProvider.AddInParameter(command, helper.Rsfdetdesp, DbType.Decimal, entity.Rsfdetdesp);
            dbProvider.AddInParameter(command, helper.Rsfdetload, DbType.Decimal, entity.Rsfdetload);
            dbProvider.AddInParameter(command, helper.Rsfdetmingen, DbType.Decimal, entity.Rsfdetmingen);
            dbProvider.AddInParameter(command, helper.Rsfdetmaxgen, DbType.Decimal, entity.Rsfdetmaxgen);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveRsfdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
     
            dbProvider.AddInParameter(command, helper.Rsfdetdesp, DbType.Decimal, entity.Rsfdetdesp);
            dbProvider.AddInParameter(command, helper.Rsfdetload, DbType.Decimal, entity.Rsfdetload);
            dbProvider.AddInParameter(command, helper.Rsfdetmingen, DbType.Decimal, entity.Rsfdetmingen);
            dbProvider.AddInParameter(command, helper.Rsfdetmaxgen, DbType.Decimal, entity.Rsfdetmaxgen);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, entity.Rsfhorcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update2(EveRsfdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate2);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rsfdetindope, DbType.Decimal, entity.Rsfdetindope);
            dbProvider.AddInParameter(command, helper.Rsfdetsub, DbType.Decimal, entity.Rsfdetsub);
            dbProvider.AddInParameter(command, helper.Rsfdetbaj, DbType.Decimal, entity.Rsfdetbaj);
            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, entity.Rsfhorcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fecha)
        {
            string sql = string.Format(helper.SqlDelete, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveRsfdetalleDTO GetById(int rsfdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rsfdetcodi, DbType.Int32, rsfdetcodi);
            EveRsfdetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveRsfdetalleDTO> List()
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
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

        public List<EveRsfdetalleDTO> GetByCriteria()
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
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

        public List<EveRsfdetalleDTO> ObtenerConfiguracion(DateTime fechaPeriodo)
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracion, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfdetalleDTO entity = new EveRsfdetalleDTO();

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iUrsnomb = dr.GetOrdinal(this.helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iGrupotipo = dr.GetOrdinal(this.helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region "COSTO OPORTUNIDAD"
        public List<EveRsfdetalleDTO> ObtenerDetalleReserva(DateTime fecha)
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
            string sql = string.Format(helper.SqlObtenerDetalleFrecuencia, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfdetalleDTO entity = helper.Create(dr);
                    //Inicio Costo Oportunidad - 20170824
                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    //Fin Costo Oportunidad - 20170824
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iSubida = dr.GetOrdinal("RSFDETSUB");
                    if (!dr.IsDBNull(iSubida)) entity.RSFDETSUB = Convert.ToDecimal(dr.GetValue(iSubida));

                    int iBajada = dr.GetOrdinal("RSFDETBAJ");
                    if (!dr.IsDBNull(iBajada)) entity.RSFDETBAJ = Convert.ToDecimal(dr.GetValue(iBajada));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public List<EveRsfdetalleDTO> ObtenerConfiguracionCO()
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerConfiguracionCO);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfdetalleDTO entity = new EveRsfdetalleDTO();

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iUrsnomb = dr.GetOrdinal(this.helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupotipo = dr.GetOrdinal(this.helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
        public List<EveRsfdetalleDTO> ObtenerUnidadesRSF(DateTime fecha)
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
            string sql = string.Format(helper.SqlObtenerUnidadesRSF, fecha.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfdetalleDTO entity = new EveRsfdetalleDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iRsfdetvalaut = dr.GetOrdinal(helper.Rsfdetvalaut);
                    if (!dr.IsDBNull(iRsfdetvalaut)) entity.Rsfdetvalaut = dr.GetDecimal(iRsfdetvalaut);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iRsfSub = dr.GetOrdinal(helper.Rsfdetsub);
                    if (!dr.IsDBNull(iRsfSub)) entity.Rsfdetsub = dr.GetDecimal(iRsfSub);


                    int iRsfBaj = dr.GetOrdinal(helper.Rsfdetbaj);
                    if (!dr.IsDBNull(iRsfBaj)) entity.Rsfdetbaj = dr.GetDecimal(iRsfBaj);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeletePorId(int id)
        {
            string sql = string.Format(helper.SqlDeletePorId, id);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        #region Modificación_RSF_05012021

        public List<EveRsfdetalleDTO> ObtenerDetalleXML(DateTime fecha)
        {
            List<EveRsfdetalleDTO> entitys = new List<EveRsfdetalleDTO>();
            string sql = string.Format(helper.SqlObtenerDetalleXML, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfdetalleDTO entity = helper.Create(dr);
                   
                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}

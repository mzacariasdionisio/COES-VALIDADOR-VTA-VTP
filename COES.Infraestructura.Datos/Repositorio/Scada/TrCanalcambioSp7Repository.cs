using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Respositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public class TrCanalcambioSp7Repository: RepositoryBase, ITrCanalcambioSp7Repository
    {
        public TrCanalcambioSp7Repository(string strConn): base(strConn)
        {
        }

        TrCanalcambioSp7Helper helper = new TrCanalcambioSp7Helper();

        public void Save(TrCanalcambioSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canalcmfeccreacion, DbType.DateTime, entity.Canalcmfeccreacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, entity.Canalnomb);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Canalabrev, DbType.String, entity.Canalabrev);
            dbProvider.AddInParameter(command, helper.Canalunidad, DbType.String, entity.Canalunidad);
            dbProvider.AddInParameter(command, helper.Canalpathb, DbType.String, entity.Canalpathb);
            dbProvider.AddInParameter(command, helper.Canalpointtype, DbType.String, entity.Canalpointtype);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Emprcodiant, DbType.Int32, entity.Emprcodiant);
            dbProvider.AddInParameter(command, helper.Zonacodiant, DbType.Int32, entity.Zonacodiant);
            dbProvider.AddInParameter(command, helper.Canalnombant, DbType.String, entity.Canalnombant);
            dbProvider.AddInParameter(command, helper.Canaliccpant, DbType.String, entity.Canaliccpant);
            dbProvider.AddInParameter(command, helper.Canalabrevant, DbType.String, entity.Canalabrevant);
            dbProvider.AddInParameter(command, helper.Canalunidadant, DbType.String, entity.Canalunidadant);
            dbProvider.AddInParameter(command, helper.Canalpathbant, DbType.String, entity.Canalpathbant);
            dbProvider.AddInParameter(command, helper.Canalpointtypeant, DbType.String, entity.Canalpointtypeant);
            dbProvider.AddInParameter(command, helper.Lastuserant, DbType.String, entity.Lastuserant);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TrCanalcambioSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, entity.Canalnomb);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Canalabrev, DbType.String, entity.Canalabrev);
            dbProvider.AddInParameter(command, helper.Canalunidad, DbType.String, entity.Canalunidad);
            dbProvider.AddInParameter(command, helper.Canalpathb, DbType.String, entity.Canalpathb);
            dbProvider.AddInParameter(command, helper.Canalpointtype, DbType.String, entity.Canalpointtype);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Emprcodiant, DbType.Int32, entity.Emprcodiant);
            dbProvider.AddInParameter(command, helper.Zonacodiant, DbType.Int32, entity.Zonacodiant);
            dbProvider.AddInParameter(command, helper.Canalnombant, DbType.String, entity.Canalnombant);
            dbProvider.AddInParameter(command, helper.Canaliccpant, DbType.String, entity.Canaliccpant);
            dbProvider.AddInParameter(command, helper.Canalabrevant, DbType.String, entity.Canalabrevant);
            dbProvider.AddInParameter(command, helper.Canalunidadant, DbType.String, entity.Canalunidadant);
            dbProvider.AddInParameter(command, helper.Canalpathbant, DbType.String, entity.Canalpathbant);
            dbProvider.AddInParameter(command, helper.Canalpointtypeant, DbType.String, entity.Canalpointtypeant);
            dbProvider.AddInParameter(command, helper.Lastuserant, DbType.String, entity.Lastuserant);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canalcmfeccreacion, DbType.DateTime, entity.Canalcmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi, DateTime canalcmfeccreacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Canalcmfeccreacion, DbType.DateTime, canalcmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrCanalcambioSp7DTO GetById(int canalcodi, DateTime canalcmfeccreacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Canalcmfeccreacion, DbType.DateTime, canalcmfeccreacion);
            TrCanalcambioSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrCanalcambioSp7DTO> List()
        {
            List<TrCanalcambioSp7DTO> entitys = new List<TrCanalcambioSp7DTO>();
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

        public List<TrCanalcambioSp7DTO> GetByCriteria()
        {
            List<TrCanalcambioSp7DTO> entitys = new List<TrCanalcambioSp7DTO>();
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
        /// Graba los datos de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public int SaveTrCanalcambioSp7Id(TrCanalcambioSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Canalcmfeccreacion==null)
                    Save(entity);
                else
                { 
                    Update(entity);                    
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<TrCanalcambioSp7DTO> BuscarOperaciones(DateTime canalCmfeccreacion, int nroPage, int pageSize)
        {
            List<TrCanalcambioSp7DTO> entitys = new List<TrCanalcambioSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, canalCmfeccreacion.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalcambioSp7DTO entity = new TrCanalcambioSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalcmfeccreacion = dr.GetOrdinal(this.helper.Canalcmfeccreacion);
                    if (!dr.IsDBNull(iCanalcmfeccreacion)) entity.Canalcmfeccreacion = dr.GetDateTime(iCanalcmfeccreacion);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(this.helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalabrev = dr.GetOrdinal(this.helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalpathb = dr.GetOrdinal(this.helper.Canalpathb);
                    if (!dr.IsDBNull(iCanalpathb)) entity.Canalpathb = dr.GetString(iCanalpathb);

                    int iCanalpointtype = dr.GetOrdinal(this.helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canalpointtype = dr.GetString(iCanalpointtype);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iEmprcodiant = dr.GetOrdinal(this.helper.Emprcodiant);
                    if (!dr.IsDBNull(iEmprcodiant)) entity.Emprcodiant = Convert.ToInt32(dr.GetValue(iEmprcodiant));

                    int iZonacodiant = dr.GetOrdinal(this.helper.Zonacodiant);
                    if (!dr.IsDBNull(iZonacodiant)) entity.Zonacodiant = Convert.ToInt32(dr.GetValue(iZonacodiant));

                    int iCanalnombant = dr.GetOrdinal(this.helper.Canalnombant);
                    if (!dr.IsDBNull(iCanalnombant)) entity.Canalnombant = dr.GetString(iCanalnombant);

                    int iCanaliccpant = dr.GetOrdinal(this.helper.Canaliccpant);
                    if (!dr.IsDBNull(iCanaliccpant)) entity.Canaliccpant = dr.GetString(iCanaliccpant);

                    int iCanalabrevant = dr.GetOrdinal(this.helper.Canalabrevant);
                    if (!dr.IsDBNull(iCanalabrevant)) entity.Canalabrevant = dr.GetString(iCanalabrevant);

                    int iCanalunidadant = dr.GetOrdinal(this.helper.Canalunidadant);
                    if (!dr.IsDBNull(iCanalunidadant)) entity.Canalunidadant = dr.GetString(iCanalunidadant);

                    int iCanalpathbant = dr.GetOrdinal(this.helper.Canalpathbant);
                    if (!dr.IsDBNull(iCanalpathbant)) entity.Canalpathbant = dr.GetString(iCanalpathbant);

                    int iCanalpointtypeant = dr.GetOrdinal(this.helper.Canalpointtypeant);
                    if (!dr.IsDBNull(iCanalpointtypeant)) entity.Canalpointtypeant = dr.GetString(iCanalpointtypeant);

                    int iLastuserant = dr.GetOrdinal(this.helper.Lastuserant);
                    if (!dr.IsDBNull(iLastuserant)) entity.Lastuserant = dr.GetString(iLastuserant);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalcambioSp7DTO> GetByFecha(DateTime fechaInicial, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<TrCanalcambioSp7DTO> entitys = new List<TrCanalcambioSp7DTO>();
            String sql = String.Format(this.helper.GetByFecha, fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalcambioSp7DTO entity = new TrCanalcambioSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalcmfeccreacion = dr.GetOrdinal(this.helper.Canalcmfeccreacion);
                    if (!dr.IsDBNull(iCanalcmfeccreacion)) entity.Canalcmfeccreacion = dr.GetDateTime(iCanalcmfeccreacion);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmpresaNombre = dr.GetOrdinal(this.helper.EmpresaNombre);
                    if (!dr.IsDBNull(iEmpresaNombre)) entity.EmpresaNombre = dr.GetString(iEmpresaNombre);

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iZonaNombre = dr.GetOrdinal(this.helper.ZonaNombre);
                    if (!dr.IsDBNull(iZonaNombre)) entity.ZonaNombre = dr.GetString(iZonaNombre);

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(this.helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalabrev = dr.GetOrdinal(this.helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalpathb = dr.GetOrdinal(this.helper.Canalpathb);
                    if (!dr.IsDBNull(iCanalpathb)) entity.Canalpathb = dr.GetString(iCanalpathb);

                    int iCanalpointtype = dr.GetOrdinal(this.helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canalpointtype = dr.GetString(iCanalpointtype);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iEmprcodiant = dr.GetOrdinal(this.helper.Emprcodiant);
                    if (!dr.IsDBNull(iEmprcodiant)) entity.Emprcodiant = Convert.ToInt32(dr.GetValue(iEmprcodiant));

                    int iEmpresaNombreant = dr.GetOrdinal(this.helper.EmpresaNombreant);
                    if (!dr.IsDBNull(iEmpresaNombreant)) entity.EmpresaNombreant = dr.GetString(iEmpresaNombreant);

                    int iZonacodiant = dr.GetOrdinal(this.helper.Zonacodiant);
                    if (!dr.IsDBNull(iZonacodiant)) entity.Zonacodiant = Convert.ToInt32(dr.GetValue(iZonacodiant));

                    int iZonaNombreant = dr.GetOrdinal(this.helper.ZonaNombreant);
                    if (!dr.IsDBNull(iZonaNombreant)) entity.ZonaNombreant = dr.GetString(iZonaNombreant);

                    int iCanalnombant = dr.GetOrdinal(this.helper.Canalnombant);
                    if (!dr.IsDBNull(iCanalnombant)) entity.Canalnombant = dr.GetString(iCanalnombant);

                    int iCanaliccpant = dr.GetOrdinal(this.helper.Canaliccpant);
                    if (!dr.IsDBNull(iCanaliccpant)) entity.Canaliccpant = dr.GetString(iCanaliccpant);

                    int iCanalabrevant = dr.GetOrdinal(this.helper.Canalabrevant);
                    if (!dr.IsDBNull(iCanalabrevant)) entity.Canalabrevant = dr.GetString(iCanalabrevant);

                    int iCanalunidadant = dr.GetOrdinal(this.helper.Canalunidadant);
                    if (!dr.IsDBNull(iCanalunidadant)) entity.Canalunidadant = dr.GetString(iCanalunidadant);

                    int iCanalpathbant = dr.GetOrdinal(this.helper.Canalpathbant);
                    if (!dr.IsDBNull(iCanalpathbant)) entity.Canalpathbant = dr.GetString(iCanalpathbant);

                    int iCanalpointtypeant = dr.GetOrdinal(this.helper.Canalpointtypeant);
                    if (!dr.IsDBNull(iCanalpointtypeant)) entity.Canalpointtypeant = dr.GetString(iCanalpointtypeant);

                    int iLastuserant = dr.GetOrdinal(this.helper.Lastuserant);
                    if (!dr.IsDBNull(iLastuserant)) entity.Lastuserant = dr.GetString(iLastuserant);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime canalCmfeccreacion)
        {
            String sql = String.Format(this.helper.TotalRegistros, canalCmfeccreacion.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}

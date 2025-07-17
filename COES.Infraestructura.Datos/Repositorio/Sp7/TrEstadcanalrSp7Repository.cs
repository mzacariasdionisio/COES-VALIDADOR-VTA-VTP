using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using COES.Dominio.Interfaces.Sp7;
using COES.Infraestructura.Datos.Helper.Sp7;
using COES.Dominio.DTO.Sp7;

namespace COES.Infraestructura.Datos.Respositorio.Sp7
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_ESTADCANALR_SP7
    /// </summary>
    public class TrEstadcanalrSp7Repository: RepositoryBase, ITrEstadcanalrSp7Repository
    {
        public TrEstadcanalrSp7Repository(string strConn): base(strConn)
        {
        }

        TrEstadcanalrSp7Helper helper = new TrEstadcanalrSp7Helper();

        public int Save(TrEstadcanalrSp7DTO entity)
        {
            DbCommand command;
                        
            if (entity.Vercodi > 0)
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            }
            else
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMinId);
            }

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Estcnlcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Estcnlfecha, DbType.DateTime, entity.Estcnlfecha);
            dbProvider.AddInParameter(command, helper.Estcnltvalido, DbType.Decimal, entity.Estcnltvalido);
            dbProvider.AddInParameter(command, helper.Estcnltcong, DbType.Decimal, entity.Estcnltcong);
            dbProvider.AddInParameter(command, helper.Estcnltindet, DbType.Decimal, entity.Estcnltindet);
            dbProvider.AddInParameter(command, helper.Estcnltnnval, DbType.Decimal, entity.Estcnltnnval);
            dbProvider.AddInParameter(command, helper.Estcnlultcalidad, DbType.Int32, entity.Estcnlultcalidad);
            dbProvider.AddInParameter(command, helper.Estcnlultcambio, DbType.DateTime, entity.Estcnlultcambio);
            dbProvider.AddInParameter(command, helper.Estcnlultcambioe, DbType.DateTime, entity.Estcnlultcambioe);
            dbProvider.AddInParameter(command, helper.Estcnlultvalor, DbType.Decimal, entity.Estcnlultvalor);
            dbProvider.AddInParameter(command, helper.Estcnltretraso, DbType.Decimal, entity.Estcnltretraso);
            dbProvider.AddInParameter(command, helper.Estcnlnumregistros, DbType.Int32, entity.Estcnlnumregistros);
            dbProvider.AddInParameter(command, helper.Estcnlverantcodi, DbType.Int32, entity.Estcnlverantcodi);
            dbProvider.AddInParameter(command, helper.Estcnlverdiaantcodi, DbType.Int32, entity.Estcnlverdiaantcodi);
            dbProvider.AddInParameter(command, helper.Estcnlingreso, DbType.String, entity.Estcnlingreso);            
            dbProvider.AddInParameter(command, helper.Estcnlusucreacion, DbType.String, entity.Estcnlusucreacion);
            dbProvider.AddInParameter(command, helper.Estcnlfeccreacion, DbType.DateTime, entity.Estcnlfeccreacion);
            dbProvider.AddInParameter(command, helper.Estcnlusumodificacion, DbType.String, entity.Estcnlusumodificacion);
            dbProvider.AddInParameter(command, helper.Estcnlfecmodificacion, DbType.DateTime, entity.Estcnlfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrEstadcanalrSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Estcnlfecha, DbType.DateTime, entity.Estcnlfecha);
            dbProvider.AddInParameter(command, helper.Estcnltvalido, DbType.Decimal, entity.Estcnltvalido);
            dbProvider.AddInParameter(command, helper.Estcnltcong, DbType.Decimal, entity.Estcnltcong);
            dbProvider.AddInParameter(command, helper.Estcnltindet, DbType.Decimal, entity.Estcnltindet);
            dbProvider.AddInParameter(command, helper.Estcnltnnval, DbType.Decimal, entity.Estcnltnnval);
            dbProvider.AddInParameter(command, helper.Estcnlultcalidad, DbType.Int32, entity.Estcnlultcalidad);
            dbProvider.AddInParameter(command, helper.Estcnlultcambio, DbType.DateTime, entity.Estcnlultcambio);
            dbProvider.AddInParameter(command, helper.Estcnlultcambioe, DbType.DateTime, entity.Estcnlultcambioe);
            dbProvider.AddInParameter(command, helper.Estcnlultvalor, DbType.Decimal, entity.Estcnlultvalor);
            dbProvider.AddInParameter(command, helper.Estcnltretraso, DbType.Decimal, entity.Estcnltretraso);
            dbProvider.AddInParameter(command, helper.Estcnlnumregistros, DbType.Int32, entity.Estcnlnumregistros);
            dbProvider.AddInParameter(command, helper.Estcnlverantcodi, DbType.Int32, entity.Estcnlverantcodi);
            dbProvider.AddInParameter(command, helper.Estcnlverdiaantcodi, DbType.Int32, entity.Estcnlverdiaantcodi);
            dbProvider.AddInParameter(command, helper.Estcnlingreso, DbType.String, entity.Estcnlingreso);            
            dbProvider.AddInParameter(command, helper.Estcnlusucreacion, DbType.String, entity.Estcnlusucreacion);
            dbProvider.AddInParameter(command, helper.Estcnlfeccreacion, DbType.DateTime, entity.Estcnlfeccreacion);
            dbProvider.AddInParameter(command, helper.Estcnlusumodificacion, DbType.String, entity.Estcnlusumodificacion);
            dbProvider.AddInParameter(command, helper.Estcnlfecmodificacion, DbType.DateTime, entity.Estcnlfecmodificacion);
            dbProvider.AddInParameter(command, helper.Estcnlcodi, DbType.Int32, entity.Estcnlcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int estcnlcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Estcnlcodi, DbType.Int32, estcnlcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int vercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, vercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrEstadcanalrSp7DTO GetById(int estcnlcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Estcnlcodi, DbType.Int32, estcnlcodi);
            TrEstadcanalrSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrEstadcanalrSp7DTO> List()
        {
            List<TrEstadcanalrSp7DTO> entitys = new List<TrEstadcanalrSp7DTO>();
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

        public List<TrEstadcanalrSp7DTO> List(int vercodi, DateTime fecha)
        {
            List<TrEstadcanalrSp7DTO> entitys = new List<TrEstadcanalrSp7DTO>();
            String sql = String.Format(this.helper.SqlListVercodiFecha,vercodi,fecha.ToString(ConstantesBase.FormatoFecha));
            
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

        public List<TrEstadcanalrSp7DTO> GetByCriteria()
        {
            List<TrEstadcanalrSp7DTO> entitys = new List<TrEstadcanalrSp7DTO>();
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
        /// Graba los datos de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public int SaveTrEstadcanalrSp7Id(TrEstadcanalrSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Estcnlcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Estcnlcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<TrEstadcanalrSp7DTO> BuscarOperaciones(int verCodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin, int nroPage, int pageSize)
        {
            List<TrEstadcanalrSp7DTO> entitys = new List<TrEstadcanalrSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, verCodi, estcnlFechaIni.ToString(ConstantesBase.FormatoFecha), estcnlFechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrEstadcanalrSp7DTO entity = new TrEstadcanalrSp7DTO();

                    int iEstcnlcodi = dr.GetOrdinal(this.helper.Estcnlcodi);
                    if (!dr.IsDBNull(iEstcnlcodi)) entity.Estcnlcodi = Convert.ToInt32(dr.GetValue(iEstcnlcodi));

                    int iVercodi = dr.GetOrdinal(this.helper.Vercodi);
                    if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iEstcnlfecha = dr.GetOrdinal(this.helper.Estcnlfecha);
                    if (!dr.IsDBNull(iEstcnlfecha)) entity.Estcnlfecha = dr.GetDateTime(iEstcnlfecha);

                    int iEstcnltvalido = dr.GetOrdinal(this.helper.Estcnltvalido);
                    if (!dr.IsDBNull(iEstcnltvalido)) entity.Estcnltvalido = dr.GetDecimal(iEstcnltvalido);

                    int iEstcnltcong = dr.GetOrdinal(this.helper.Estcnltcong);
                    if (!dr.IsDBNull(iEstcnltcong)) entity.Estcnltcong = dr.GetDecimal(iEstcnltcong);

                    int iEstcnltindet = dr.GetOrdinal(this.helper.Estcnltindet);
                    if (!dr.IsDBNull(iEstcnltindet)) entity.Estcnltindet = dr.GetDecimal(iEstcnltindet);

                    int iEstcnltnnval = dr.GetOrdinal(this.helper.Estcnltnnval);
                    if (!dr.IsDBNull(iEstcnltnnval)) entity.Estcnltnnval = dr.GetDecimal(iEstcnltnnval);

                    int iEstcnlultcalidad = dr.GetOrdinal(this.helper.Estcnlultcalidad);
                    if (!dr.IsDBNull(iEstcnlultcalidad)) entity.Estcnlultcalidad = Convert.ToInt32(dr.GetValue(iEstcnlultcalidad));

                    int iEstcnlultcambio = dr.GetOrdinal(this.helper.Estcnlultcambio);
                    if (!dr.IsDBNull(iEstcnlultcambio)) entity.Estcnlultcambio = dr.GetDateTime(iEstcnlultcambio);

                    int iEstcnlultcambioe = dr.GetOrdinal(this.helper.Estcnlultcambioe);
                    if (!dr.IsDBNull(iEstcnlultcambioe)) entity.Estcnlultcambioe = dr.GetDateTime(iEstcnlultcambioe);

                    int iEstcnlultvalor = dr.GetOrdinal(this.helper.Estcnlultvalor);
                    if (!dr.IsDBNull(iEstcnlultvalor)) entity.Estcnlultvalor = dr.GetDecimal(iEstcnlultvalor);

                    int iEstcnltretraso = dr.GetOrdinal(this.helper.Estcnltretraso);
                    if (!dr.IsDBNull(iEstcnltretraso)) entity.Estcnltretraso = dr.GetDecimal(iEstcnltretraso);

                    int iEstcnlnumregistros = dr.GetOrdinal(this.helper.Estcnlnumregistros);
                    if (!dr.IsDBNull(iEstcnlnumregistros)) entity.Estcnlnumregistros = Convert.ToInt32(dr.GetValue(iEstcnlnumregistros));

                    int iEstcnlverantcodi = dr.GetOrdinal(this.helper.Estcnlverantcodi);
                    if (!dr.IsDBNull(iEstcnlverantcodi)) entity.Estcnlverantcodi = Convert.ToInt32(dr.GetValue(iEstcnlverantcodi));

                    int iEstcnlverdiaantcodi = dr.GetOrdinal(this.helper.Estcnlverdiaantcodi);
                    if (!dr.IsDBNull(iEstcnlverdiaantcodi)) entity.Estcnlverdiaantcodi = Convert.ToInt32(dr.GetValue(iEstcnlverdiaantcodi));

                    int iEstcnlingreso = dr.GetOrdinal(this.helper.Estcnlingreso);
                    if (!dr.IsDBNull(iEstcnlingreso)) entity.Estcnlingreso = dr.GetString(iEstcnlingreso);

                    int iEstcnlusucreacion = dr.GetOrdinal(this.helper.Estcnlusucreacion);
                    if (!dr.IsDBNull(iEstcnlusucreacion)) entity.Estcnlusucreacion = dr.GetString(iEstcnlusucreacion);

                    int iEstcnlfeccreacion = dr.GetOrdinal(this.helper.Estcnlfeccreacion);
                    if (!dr.IsDBNull(iEstcnlfeccreacion)) entity.Estcnlfeccreacion = dr.GetDateTime(iEstcnlfeccreacion);

                    int iEstcnlusumodificacion = dr.GetOrdinal(this.helper.Estcnlusumodificacion);
                    if (!dr.IsDBNull(iEstcnlusumodificacion)) entity.Estcnlusumodificacion = dr.GetString(iEstcnlusumodificacion);

                    int iEstcnlfecmodificacion = dr.GetOrdinal(this.helper.Estcnlfecmodificacion);
                    if (!dr.IsDBNull(iEstcnlfecmodificacion)) entity.Estcnlfecmodificacion = dr.GetDateTime(iEstcnlfecmodificacion);

                    int iVerfechaini = dr.GetOrdinal(this.helper.Verfechaini);
                    if (!dr.IsDBNull(iVerfechaini)) entity.Verfechaini = dr.GetDateTime(iVerfechaini);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrEstadcanalrSp7DTO> BuscarOperaciones(int verCodi,int emprcodi,int zonacodi,int canalcodi,int segundosDia, DateTime estcnlFechaIni, DateTime estcnlFechaFin, int nroPage, int pageSize)
        {
            List<TrEstadcanalrSp7DTO> entitys = new List<TrEstadcanalrSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListadoClasif, verCodi, emprcodi, zonacodi, canalcodi, segundosDia,estcnlFechaIni.ToString(ConstantesBase.FormatoFecha), estcnlFechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrEstadcanalrSp7DTO entity = new TrEstadcanalrSp7DTO();

                    int iEstcnlcodi = dr.GetOrdinal(this.helper.Estcnlcodi);
                    if (!dr.IsDBNull(iEstcnlcodi)) entity.Estcnlcodi = Convert.ToInt32(dr.GetValue(iEstcnlcodi));

                    int iVercodi = dr.GetOrdinal(this.helper.Vercodi);
                    if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iEstcnlfecha = dr.GetOrdinal(this.helper.Estcnlfecha);
                    if (!dr.IsDBNull(iEstcnlfecha)) entity.Estcnlfecha = dr.GetDateTime(iEstcnlfecha);

                    int iEstcnltvalido = dr.GetOrdinal(this.helper.Estcnltvalido);
                    if (!dr.IsDBNull(iEstcnltvalido)) entity.Estcnltvalido = dr.GetDecimal(iEstcnltvalido);

                    int iEstcnltcong = dr.GetOrdinal(this.helper.Estcnltcong);
                    if (!dr.IsDBNull(iEstcnltcong)) entity.Estcnltcong = dr.GetDecimal(iEstcnltcong);

                    int iEstcnltindet = dr.GetOrdinal(this.helper.Estcnltindet);
                    if (!dr.IsDBNull(iEstcnltindet)) entity.Estcnltindet = dr.GetDecimal(iEstcnltindet);

                    int iEstcnltnnval = dr.GetOrdinal(this.helper.Estcnltnnval);
                    if (!dr.IsDBNull(iEstcnltnnval)) entity.Estcnltnnval = dr.GetDecimal(iEstcnltnnval);

                    int iEstcnlultcalidad = dr.GetOrdinal(this.helper.Estcnlultcalidad);
                    if (!dr.IsDBNull(iEstcnlultcalidad)) entity.Estcnlultcalidad = Convert.ToInt32(dr.GetValue(iEstcnlultcalidad));

                    int iEstcnlultcambio = dr.GetOrdinal(this.helper.Estcnlultcambio);
                    if (!dr.IsDBNull(iEstcnlultcambio)) entity.Estcnlultcambio = dr.GetDateTime(iEstcnlultcambio);

                    int iEstcnlultcambioe = dr.GetOrdinal(this.helper.Estcnlultcambioe);
                    if (!dr.IsDBNull(iEstcnlultcambioe)) entity.Estcnlultcambioe = dr.GetDateTime(iEstcnlultcambioe);

                    int iEstcnlultvalor = dr.GetOrdinal(this.helper.Estcnlultvalor);
                    if (!dr.IsDBNull(iEstcnlultvalor)) entity.Estcnlultvalor = dr.GetDecimal(iEstcnlultvalor);

                    int iEstcnltretraso = dr.GetOrdinal(this.helper.Estcnltretraso);
                    if (!dr.IsDBNull(iEstcnltretraso)) entity.Estcnltretraso = dr.GetDecimal(iEstcnltretraso);

                    int iEstcnlnumregistros = dr.GetOrdinal(this.helper.Estcnlnumregistros);
                    if (!dr.IsDBNull(iEstcnlnumregistros)) entity.Estcnlnumregistros = Convert.ToInt32(dr.GetValue(iEstcnlnumregistros));

                    int iEstcnlverantcodi = dr.GetOrdinal(this.helper.Estcnlverantcodi);
                    if (!dr.IsDBNull(iEstcnlverantcodi)) entity.Estcnlverantcodi = Convert.ToInt32(dr.GetValue(iEstcnlverantcodi));

                    int iEstcnlverdiaantcodi = dr.GetOrdinal(this.helper.Estcnlverdiaantcodi);
                    if (!dr.IsDBNull(iEstcnlverdiaantcodi)) entity.Estcnlverdiaantcodi = Convert.ToInt32(dr.GetValue(iEstcnlverdiaantcodi));

                    int iEstcnlingreso = dr.GetOrdinal(this.helper.Estcnlingreso);
                    if (!dr.IsDBNull(iEstcnlingreso)) entity.Estcnlingreso = dr.GetString(iEstcnlingreso);

                    int iEstcnlusucreacion = dr.GetOrdinal(this.helper.Estcnlusucreacion);
                    if (!dr.IsDBNull(iEstcnlusucreacion)) entity.Estcnlusucreacion = dr.GetString(iEstcnlusucreacion);

                    int iEstcnlfeccreacion = dr.GetOrdinal(this.helper.Estcnlfeccreacion);
                    if (!dr.IsDBNull(iEstcnlfeccreacion)) entity.Estcnlfeccreacion = dr.GetDateTime(iEstcnlfeccreacion);

                    int iEstcnlusumodificacion = dr.GetOrdinal(this.helper.Estcnlusumodificacion);
                    if (!dr.IsDBNull(iEstcnlusumodificacion)) entity.Estcnlusumodificacion = dr.GetString(iEstcnlusumodificacion);

                    int iEstcnlfecmodificacion = dr.GetOrdinal(this.helper.Estcnlfecmodificacion);
                    if (!dr.IsDBNull(iEstcnlfecmodificacion)) entity.Estcnlfecmodificacion = dr.GetDateTime(iEstcnlfecmodificacion);


                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(this.helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iEmprenomb = dr.GetOrdinal(this.helper.Emprenomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);

                    int iZonanomb = dr.GetOrdinal(this.helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int verCodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin)
        {
            String sql = String.Format(this.helper.TotalRegistros, verCodi, estcnlFechaIni.ToString(ConstantesBase.FormatoFecha), estcnlFechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public int ObtenerNroFilas(int verCodi, int emprcodi, int zonacodi, int canalcodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin)
        {
            String sql = String.Format(this.helper.TotalRegistrosClasif, verCodi, emprcodi, zonacodi, canalcodi,estcnlFechaIni.ToString(ConstantesBase.FormatoFecha), estcnlFechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<TrEstadcanalrSp7DTO> GetDispDiaSignal(DateTime fecha, int emprcodi)
        {

            string query = string.Format(helper.SqlGetDispDiaSignal, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<TrEstadcanalrSp7DTO> entitys = new List<TrEstadcanalrSp7DTO>();
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    TrEstadcanalrSp7DTO entity = new TrEstadcanalrSp7DTO();


                    int iZona = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZona)) entity.Zonanomb = dr.GetString(iZona);

                    int iDispo = dr.GetOrdinal(helper.Estcnltvalido);
                    if (!dr.IsDBNull(iDispo)) entity.Estcnltvalido = dr.GetDecimal(iDispo);

                    int iNombCanal = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iNombCanal)) entity.Canalnomb = dr.GetString(iNombCanal);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);


                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iUnidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iZona)) entity.Canalunidad = dr.GetString(iUnidad);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

    }
}

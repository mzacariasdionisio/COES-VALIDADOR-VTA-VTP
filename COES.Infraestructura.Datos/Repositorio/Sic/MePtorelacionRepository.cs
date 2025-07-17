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
    /// Clase de acceso a datos de la tabla ME_PTORELACION
    /// </summary>
    public class MePtorelacionRepository : RepositoryBase, IMePtorelacionRepository
    {
        public MePtorelacionRepository(string strConn)
            : base(strConn)
        {
        }

        MePtorelacionHelper helper = new MePtorelacionHelper();

        public int Save(MePtorelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptorelcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ptorelpunto1, DbType.Int32, entity.Ptorelpunto1);
            dbProvider.AddInParameter(command, helper.Ptorelpunto2, DbType.Int32, entity.Ptorelpunto2);
            dbProvider.AddInParameter(command, helper.Ptoreltipo, DbType.String, entity.Ptoreltipo);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MePtorelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ptorelpunto1, DbType.Int32, entity.Ptorelpunto1);
            dbProvider.AddInParameter(command, helper.Ptorelpunto2, DbType.Int32, entity.Ptorelpunto2);
            dbProvider.AddInParameter(command, helper.Ptoreltipo, DbType.String, entity.Ptoreltipo);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Ptorelcodi, DbType.Int32, entity.Ptorelcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MePtorelacionDTO GetById(int ptorelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptorelcodi, DbType.Int32, ptorelcodi);
            MePtorelacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MePtorelacionDTO> List(int idCentral)
        {
            List<MePtorelacionDTO> entitys = new List<MePtorelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Centralcodi, DbType.Int32, idCentral);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtorelacionDTO entity = new MePtorelacionDTO();

                    int iOriglectcodi = dr.GetOrdinal(helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iSeleccion = dr.GetOrdinal(helper.Seleccion);
                    if (!dr.IsDBNull(iSeleccion)) entity.Seleccion = Convert.ToInt32(dr.GetValue(iSeleccion));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtorelacionDTO> GetByCriteria(int? idEmpresa, int? idCentral)
        {
            List<MePtorelacionDTO> entitys = new List<MePtorelacionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idEmpresa, idCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtorelacionDTO entity = new MePtorelacionDTO();

                    int iCentralcodi = dr.GetOrdinal(helper.Centralcodi);
                    if (!dr.IsDBNull(iCentralcodi)) entity.Centralcodi = Convert.ToInt32(dr.GetValue(iCentralcodi));

                    int iCentralnomb = dr.GetOrdinal(helper.Centralnomb);
                    if (!dr.IsDBNull(iCentralnomb)) entity.Centralnomb = dr.GetString(iCentralnomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iOriglectcodi = dr.GetOrdinal(helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));                        

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresas()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarCentrales(int idEmpresa, DateTime fechaPeriodo)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = string.Format(helper.SqlObtenerCentrales, idEmpresa, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtorelacionDTO> ObtenerPuntosRelacion(int? idEmpresa, int? idCentral, DateTime fechaPeriodo)
        {
            List<MePtorelacionDTO> entitys = new List<MePtorelacionDTO>();
            string sql = string.Format(helper.SqlObtenerPuntosMedicion, idEmpresa, idCentral, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtorelacionDTO entity = new MePtorelacionDTO();

                    int iOriglectcodi = dr.GetOrdinal(helper.Origlectcodi);
                    if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public MeMedicion48DTO ObtenerDatosDespacho(DateTime fecha, string ptomedicodi)
        {
            MeMedicion48DTO entity = null;
            string sql = string.Format(helper.SqlObtenerDatosDespacho, ptomedicodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new MeMedicion48DTO();

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal(helper.CampoH + i);
                        if(!dr.IsDBNull(iOrdinal))entity.GetType().GetProperty(helper.CampoH + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }
                }
            }

            return entity;
        }

        public List<MePtorelacionDTO> ObtenerPuntosRPF(DateTime fechaPeriodo)
        {
            List<MePtorelacionDTO> entitys = new List<MePtorelacionDTO>();
            string sql = string.Format(helper.SqlObtenerPuntosRPF, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtorelacionDTO entity = new MePtorelacionDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

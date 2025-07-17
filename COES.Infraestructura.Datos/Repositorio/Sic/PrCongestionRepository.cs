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
    /// Clase de acceso a datos de la tabla PR_CONGESTION
    /// </summary>
    public class PrCongestionRepository : RepositoryBase, IPrCongestionRepository
    {
        public PrCongestionRepository(string strConn)
            : base(strConn)
        {
        }

        PrCongestionHelper helper = new PrCongestionHelper();

        public int Save(PrCongestionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Congesfecinicio, DbType.DateTime, entity.Congesfecinicio);
            dbProvider.AddInParameter(command, helper.Congesfecfin, DbType.DateTime, entity.Congesfecfin);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Indtipo, DbType.String, entity.Indtipo);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Congesmotivo, DbType.String, entity.Congesmotivo);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrCongestionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Congesfecinicio, DbType.DateTime, entity.Congesfecinicio);
            dbProvider.AddInParameter(command, helper.Congesfecfin, DbType.DateTime, entity.Congesfecfin);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Indtipo, DbType.String, entity.Indtipo);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Congesmotivo, DbType.String, entity.Congesmotivo);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, entity.Congescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int congescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, congescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrCongestionDTO GetById(int congescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Congescodi, DbType.Int32, congescodi);
            PrCongestionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrCongestionDTO> List()
        {
            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
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

        public List<PrCongestionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            string sql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
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

        public List<PrCongestionDTO> ObtenerCongestionSimple(DateTime fechaInicio, DateTime fechaFin)
        {
            string sql = string.Format(helper.SqlObtenerCongestionSimple, fechaInicio.ToString(ConstantesBase.FormatoFecha),
               fechaFin.ToString(ConstantesBase.FormatoFecha));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrCongestionDTO> ObtenerCongestionConjunto(DateTime fechaInicio, DateTime fechaFin)
        {
            string sql = string.Format(helper.SqlObtenerCongestionConjunto, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                   fechaFin.ToString(ConstantesBase.FormatoFecha));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = helper.Create(dr);

                    int iGrulinnombre = dr.GetOrdinal(helper.Grulinnombre);
                    if (!dr.IsDBNull(iGrulinnombre)) entity.Grulinnombre = dr.GetString(iGrulinnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrCongestionDTO> ObtenerCongestionRegistro(DateTime fechaProceso)
        {
            string sql = string.Format(helper.SqlObtenerCongestionRegistro, fechaProceso.ToString(ConstantesBase.FormatoFechaHora));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = new PrCongestionDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iConfigcodi = dr.GetOrdinal(helper.Configcodi);
                    if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

                    int iNodobarra1 = dr.GetOrdinal(helper.Nodobarra1);
                    if (!dr.IsDBNull(iNodobarra1)) entity.Nodobarra1 = dr.GetString(iNodobarra1);

                    int iNodobarra2 = dr.GetOrdinal(helper.Nodobarra2);
                    if (!dr.IsDBNull(iNodobarra2)) entity.Nodobarra2 = dr.GetString(iNodobarra2);

                    int iNodobarra3 = dr.GetOrdinal(helper.Nodobarra3);
                    if (!dr.IsDBNull(iNodobarra3)) entity.Nodobarra3 = dr.GetString(iNodobarra3);

                    int iIdEms = dr.GetOrdinal(helper.Idems);
                    if (!dr.IsDBNull(iIdEms)) entity.NombLinea = dr.GetString(iIdEms);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iNombretna1 = dr.GetOrdinal(helper.Nombretna1);
                    if (!dr.IsDBNull(iNombretna1)) entity.Nombretna1 = dr.GetString(iNombretna1);

                    int iNombretna2 = dr.GetOrdinal(helper.Nombretna2);
                    if (!dr.IsDBNull(iNombretna2)) entity.Nombretna2 = dr.GetString(iNombretna2);

                    int iNombretna3 = dr.GetOrdinal(helper.Nombretna3);
                    if (!dr.IsDBNull(iNombretna3)) entity.Nombretna3 = dr.GetString(iNombretna3);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrCongestionDTO> ObtenerCongestionConjuntoRegistro(DateTime fechaProceso, string tipo)
        {
            string sql = string.Format(helper.SqlObtenerCongestionConjuntoRegistro, 
                fechaProceso.ToString(ConstantesBase.FormatoFechaHora), tipo);

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = new PrCongestionDTO();

                    int iConfigcodi = dr.GetOrdinal(helper.Configcodi);
                    if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

                    int iGrulincodi = dr.GetOrdinal(helper.Grulincodi);
                    if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

                    int iNodobarra1 = dr.GetOrdinal(helper.Nodobarra1);
                    if (!dr.IsDBNull(iNodobarra1)) entity.Nodobarra1 = dr.GetString(iNodobarra1);

                    int iNodobarra2 = dr.GetOrdinal(helper.Nodobarra2);
                    if (!dr.IsDBNull(iNodobarra2)) entity.Nodobarra2 = dr.GetString(iNodobarra2);

                    int iNodobarra3 = dr.GetOrdinal(helper.Nodobarra3);
                    if (!dr.IsDBNull(iNodobarra3)) entity.Nodobarra3 = dr.GetString(iNodobarra3);

                    int iIdEms = dr.GetOrdinal(helper.Idems);
                    if (!dr.IsDBNull(iIdEms)) entity.NombLinea = dr.GetString(iIdEms);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iNombretna1 = dr.GetOrdinal(helper.Nombretna1);
                    if (!dr.IsDBNull(iNombretna1)) entity.Nombretna1 = dr.GetString(iNombretna1);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrCongestionDTO> ObtenerCongestionRegionSeguridad(DateTime fechaProceso)
        {
            string sql = string.Format(helper.SqlObtenerCongestionRegionSeguridad, fechaProceso.ToString(ConstantesBase.FormatoFechaHora));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = new PrCongestionDTO();

                    int iRegsegcodi = dr.GetOrdinal(helper.Regsegcodi);
                    if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

                    int iRegsegvalorm = dr.GetOrdinal(helper.Regsegvalorm);
                    if (!dr.IsDBNull(iRegsegvalorm)) entity.Regsegvalorm = dr.GetDecimal(iRegsegvalorm);

                    int iRegsegdirec = dr.GetOrdinal(helper.Regsegdirec);
                    if (!dr.IsDBNull(iRegsegdirec)) entity.Regsegdirec = dr.GetString(iRegsegdirec);

                    int iRegsegnombre = dr.GetOrdinal(helper.Regsegnombre);
                    if (!dr.IsDBNull(iRegsegnombre)) entity.Regsegnombre = dr.GetString(iRegsegnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrCongestionDTO> ObtenerCongestion(DateTime fechaInicio, DateTime fechaFin)
        {
            string sql = string.Format(helper.SqlObtenerCongestion, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = new PrCongestionDTO();

                    int iCongescodi = dr.GetOrdinal(helper.Congescodi);
                    if (!dr.IsDBNull(iCongescodi)) entity.Congescodi = Convert.ToInt32(dr.GetValue(iCongescodi));

                    int iCongesfecinicio = dr.GetOrdinal(this.helper.Congesfecinicio);
                    if (!dr.IsDBNull(iCongesfecinicio)) entity.Congesfecinicio = dr.GetDateTime(iCongesfecinicio);

                    int iCongesfecfin = dr.GetOrdinal(this.helper.Congesfecfin);
                    if (!dr.IsDBNull(iCongesfecfin)) entity.Congesfecfin = dr.GetDateTime(iCongesfecfin);

                    int iConfigcodi = dr.GetOrdinal(helper.Configcodi);
                    if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

                    int iGrulincodi = dr.GetOrdinal(helper.Grulincodi);
                    if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

                    int iCongesmotivo = dr.GetOrdinal(helper.Congesmotivo);
                    if (!dr.IsDBNull(iCongesmotivo)) entity.Congesmotivo = dr.GetString(iCongesmotivo);

                    int iIccodi = dr.GetOrdinal(helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

                    int iRegsegcodi = dr.GetOrdinal(helper.Regsegcodi);
                    if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

                    int iHopcodi = dr.GetOrdinal(helper.Hopcodi);
                    if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

                    int iIndtipo = dr.GetOrdinal(helper.Indtipo);
                    if (!dr.IsDBNull(iIndtipo)) entity.Indtipo = dr.GetString(iIndtipo);

                    int iGrulintipo = dr.GetOrdinal(helper.Grulintipo);
                    if (!dr.IsDBNull(iGrulintipo)) entity.Grulintipo = dr.GetString(iGrulintipo);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrCongestionDTO> ListaCongestionConjunto(DateTime fechaInicio, DateTime fechaFin, string Indtipo)
        {
            string sql = string.Format(helper.SqlListaCongestionConjunto, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), Indtipo);

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCongestionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGrulinnombre = dr.GetOrdinal(helper.Grulinnombre);
                    if (!dr.IsDBNull(iGrulinnombre)) entity.Grulinnombre = dr.GetString(iGrulinnombre);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iAreaoperativa = dr.GetOrdinal(helper.Areaoperativa);
                    if (!dr.IsDBNull(iAreaoperativa)) entity.Areaoperativa = dr.GetString(iAreaoperativa);

                    int iRegsegnombre = dr.GetOrdinal(helper.Regsegnombre);
                    if (!dr.IsDBNull(iRegsegnombre)) entity.Regsegnombre = dr.GetString(iRegsegnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public bool VerificarExistenciaCongestion(int configcodi, DateTime fecha)
        {
            string sql = string.Format(helper.SqlVerificarExistenciaCongestion, configcodi, fecha.ToString(ConstantesBase.FormatoFecha));

            List<PrCongestionDTO> entitys = new List<PrCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            Object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                int count = Convert.ToInt32(result);

                if (count > 0) return true;
                else return false;
            }

            return false;           
        }
    }
}

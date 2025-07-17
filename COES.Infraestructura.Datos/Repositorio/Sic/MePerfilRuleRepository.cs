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
    /// Clase de acceso a datos de la tabla ME_PERFIL_RULE
    /// </summary>
    public class MePerfilRuleRepository: RepositoryBase, IMePerfilRuleRepository
    {
        public MePerfilRuleRepository(string strConn): base(strConn)
        {
        }

        MePerfilRuleHelper helper = new MePerfilRuleHelper();

        public int Save(MePerfilRuleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prrupref, DbType.String, entity.Prrupref);
            dbProvider.AddInParameter(command, helper.Prruabrev, DbType.String, entity.Prruabrev);
            dbProvider.AddInParameter(command, helper.Prrudetalle, DbType.String, entity.Prrudetalle);
            dbProvider.AddInParameter(command, helper.Prruformula, DbType.String, entity.Prruformula);
            dbProvider.AddInParameter(command, helper.Prruactiva, DbType.String, entity.Prruactiva);
            dbProvider.AddInParameter(command, helper.Prrulastuser, DbType.String, entity.Prrulastuser);
            dbProvider.AddInParameter(command, helper.Prrulastdate, DbType.DateTime, entity.Prrulastdate);
            dbProvider.AddInParameter(command, helper.Prruind, DbType.String, entity.Prruind);
            dbProvider.AddInParameter(command, helper.Prrufirstuser, DbType.String, entity.Prrufirstuser);
            dbProvider.AddInParameter(command, helper.Prrufirstdate, DbType.DateTime, entity.Prrufirstdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MePerfilRuleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);                       
            
            dbProvider.AddInParameter(command, helper.Prruabrev, DbType.String, entity.Prruabrev);
            dbProvider.AddInParameter(command, helper.Prrudetalle, DbType.String, entity.Prrudetalle);
            dbProvider.AddInParameter(command, helper.Prruformula, DbType.String, entity.Prruformula);
            dbProvider.AddInParameter(command, helper.Prruactiva, DbType.String, entity.Prruactiva);
            dbProvider.AddInParameter(command, helper.Prrulastuser, DbType.String, entity.Prrulastuser);
            dbProvider.AddInParameter(command, helper.Prrulastdate, DbType.DateTime, entity.Prrulastdate);
            dbProvider.AddInParameter(command, helper.Prruind, DbType.String, entity.Prruind);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, entity.Prrucodi);
           
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int prrucodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
                        
            dbProvider.AddInParameter(command, helper.Prrulastuser, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Prrulastdate, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, prrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MePerfilRuleDTO GetById(int prrucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, prrucodi);
            MePerfilRuleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MePerfilRuleDTO> List()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
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

        public List<MePerfilRuleDTO> GetByCriteria(int area, string areaOperativa)
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            string query = string.Format(helper.SqlGetByCriteria, area, areaOperativa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> GetByCriteriaPorUsuario(int area, string areaOperativa, string UserLog)
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            string query = string.Format(helper.SqlGetByCriteriaPorUsuario, area, areaOperativa, UserLog);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> ObtenerPuntosEjecutado()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosEjecutado);
         
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iPtoNomb = dr.GetOrdinal(helper.PtoNomb);
                    if (!dr.IsDBNull(iPtoNomb)) entity.PtoNomb = dr.GetString(iPtoNomb);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> ObtenerPuntosDemanda()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosDemanda);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iPtoNomb = dr.GetOrdinal(helper.PtoNomb);
                    if (!dr.IsDBNull(iPtoNomb)) entity.PtoNomb = dr.GetString(iPtoNomb);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));                        

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MePerfilRuleDTO> ObtenerPuntosScada()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosSCADA);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


       

        public List<MePerfilRuleDTO> ObtenerPuntosMedidoresGeneracion() 
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosMedidoresGeneracion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> ObtenePuntosDemandaULyD()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosDemandaULyD);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string ObtenerNombrePunto(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePunto);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }

        public string ObtenerNombrePuntoDemanda(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePuntoDemanda);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }

        public string ObtenerNombrePuntoScada(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePuntoScada);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }

        public string ObtenerNombrePuntoMedidoresGeneracion(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePuntoMedidoresGeneracion);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }

        public List<MePerfilRuleDTO> ObtenerPuntosScadaSP7()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosSCADASP7);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi); 
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public string ObtenerNombrePuntoScadaSP7(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePuntoScadaSP7);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }

        public string ObtenerNombrePuntoPR16(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePuntoPR16);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }
        

        #region PR5
        public List<MePerfilRuleDTO> ObtenerPuntosPR5(int origlectcodi)
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            string query = string.Format(helper.SqlObtenerPuntosP5, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iPtoNomb = dr.GetOrdinal(helper.PtoNomb);
                    if (!dr.IsDBNull(iPtoNomb)) entity.PtoNomb = dr.GetString(iPtoNomb);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string ObtenerNombrePuntoPR5(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNombrePunto);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return result.ToString();
            }

            return string.Empty;
        }
        #endregion

        //Assetec 20220308 Inicio
        public List<MePerfilRuleDTO> ObtenerPuntosServiciosAuxiliares()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosServiciosAuxiliares);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiNomb = dr.GetOrdinal(helper.EquiNomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iPtoMediCodi = dr.GetOrdinal(helper.PtoMediCodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoDescripcion = dr.GetOrdinal(helper.PtoDescripcion);
                    if (!dr.IsDBNull(iPtoDescripcion)) entity.PtoDescripcion = dr.GetString(iPtoDescripcion);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iAreaCodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //Assetec 20220308 Fin
    }
}

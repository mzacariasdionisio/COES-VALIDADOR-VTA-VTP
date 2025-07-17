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
    /// Clase de acceso a datos de la tabla WB_GENERACIONRER
    /// </summary>
    public class WbGeneracionrerRepository: RepositoryBase, IWbGeneracionrerRepository
    {
        public WbGeneracionrerRepository(string strConn): base(strConn)
        {
        }

        WbGeneracionrerHelper helper = new WbGeneracionrerHelper();

        public void Save(WbGeneracionrerDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Userupdate, DbType.String, entity.Userupdate);
            dbProvider.AddInParameter(command, helper.Usercreate, DbType.String, entity.Usercreate);
            dbProvider.AddInParameter(command, helper.Fecupdate, DbType.DateTime, entity.Fecupdate);
            dbProvider.AddInParameter(command, helper.Feccreate, DbType.DateTime, entity.Feccreate);
            dbProvider.AddInParameter(command, helper.Genrermax, DbType.Decimal, entity.Genrermax);
            dbProvider.AddInParameter(command, helper.Genrermin, DbType.Decimal, entity.Genrermin);

            dbProvider.ExecuteNonQuery(command);
           
        }

        public void Update(WbGeneracionrerDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Userupdate, DbType.String, entity.Userupdate);
            dbProvider.AddInParameter(command, helper.Usercreate, DbType.String, entity.Usercreate);
            dbProvider.AddInParameter(command, helper.Fecupdate, DbType.DateTime, entity.Fecupdate);
            dbProvider.AddInParameter(command, helper.Feccreate, DbType.DateTime, entity.Feccreate);
            dbProvider.AddInParameter(command, helper.Genrermax, DbType.Decimal, entity.Genrermax);
            dbProvider.AddInParameter(command, helper.Genrermin, DbType.Decimal, entity.Genrermin);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public int ValidarExistencia(int ptoMediCodi) 
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistencia);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptoMediCodi);
            
            object result = dbProvider.ExecuteScalar(command);

            int count = 0;
            if (result != null) count = Convert.ToInt32(result);

            return count;
        }

        public int ValidarExistenciaUnidad(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistenciaUnidad);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            int count = 0;
            if (result != null) count = Convert.ToInt32(result);

            return count;
        }

        public int ValidarExistenciaGeneral(int ptoMediCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistenciaGeneral);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptoMediCodi);

            object result = dbProvider.ExecuteScalar(command);

            int count = 0;
            if (result != null) count = Convert.ToInt32(result);

            return count;
        }



        public void Delete(int ptomedicodi, string lastUser, DateTime lastDate)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Userupdate, DbType.String, lastUser);
            dbProvider.AddInParameter(command, helper.Fecupdate, DbType.DateTime, lastDate);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbGeneracionrerDTO GetById(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            WbGeneracionrerDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbGeneracionrerDTO> List()
        {
            List<WbGeneracionrerDTO> entitys = new List<WbGeneracionrerDTO>();
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

        public List<WbGeneracionrerDTO> GetByCriteria()
        {
            List<WbGeneracionrerDTO> entitys = new List<WbGeneracionrerDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbGeneracionrerDTO entity = this.helper.Create(dr);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iCentral = dr.GetOrdinal(this.helper.CentralNomb);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iIndicador = dr.GetOrdinal(this.helper.IndPorCentral);
                    if (!dr.IsDBNull(iIndicador)) entity.IndPorCentral = dr.GetString(iIndicador);

                    int iEmprCodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbGeneracionrerDTO> ObtenerPuntosEmpresas()
        {
            List<WbGeneracionrerDTO> entitys = new List<WbGeneracionrerDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbGeneracionrerDTO entity = new WbGeneracionrerDTO();

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iEmprCodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbGeneracionrerDTO> ObtenerPuntosCentrales(int idEmpresa)
        {
            List<WbGeneracionrerDTO> entitys = new List<WbGeneracionrerDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCentrales);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbGeneracionrerDTO entity = new WbGeneracionrerDTO();
                     
                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);                    
                    
                    int iPtoMediCodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    entitys.Add(entity);
                }
            }
            return entitys;            
        }

        public List<WbGeneracionrerDTO> ObtenerPuntosUnidades(int ptoCentral)
        {
            List<WbGeneracionrerDTO> entitys = new List<WbGeneracionrerDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerUnidades);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptoCentral);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbGeneracionrerDTO entity = new WbGeneracionrerDTO();

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);                                       

                    int iPtoMediCodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    entitys.Add(entity);
                }
            }
            
            return entitys;
        }

        public List<WbGeneracionrerDTO> ObtenerPuntoFormato(int idEmpresa)
        {
            List<WbGeneracionrerDTO> entitys = new List<WbGeneracionrerDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntoFormato);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbGeneracionrerDTO entity = this.helper.Create(dr);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EquiNomb = dr.GetString(iEquiNomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iCentral = dr.GetOrdinal(this.helper.CentralNomb);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iIndicador = dr.GetOrdinal(this.helper.IndPorCentral);
                    if (!dr.IsDBNull(iIndicador)) entity.IndPorCentral = dr.GetString(iIndicador);

                    int iCentralCodi = dr.GetOrdinal(this.helper.CentralCodi);
                    if (!dr.IsDBNull(iCentralCodi)) entity.CentralCodi = Convert.ToInt32(dr.GetValue(iCentralCodi));

                    int iPtodespacho = dr.GetOrdinal(this.helper.Ptodespacho);
                    if (!dr.IsDBNull(iPtodespacho)) entity.Ptodespacho = Convert.ToInt32(dr.GetValue(iPtodespacho));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string ObtenerEmpresaPorUsuario(string userLogin)
        {
            string query = String.Format(helper.SqlObtenerEmpresaUsuario, userLogin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) return result.ToString();

            return string.Empty;
        }

        public void GrabarConfiguracion(int ptomedicodi, decimal? minimo, decimal? maximo, string lastuser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGrabarConfiguracion);
            dbProvider.AddInParameter(command, helper.Genrermin, DbType.Decimal, minimo);
            dbProvider.AddInParameter(command, helper.Genrermax, DbType.Decimal, maximo);
            dbProvider.AddInParameter(command, helper.Userupdate, DbType.String, lastuser);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}

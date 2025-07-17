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

namespace COES.Infraestructura.Datos.Respositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_VERSION_SP7
    /// </summary>
    public class TrVersionSp7Repository: RepositoryBase, ITrVersionSp7Repository
    {
        public TrVersionSp7Repository(string strConn): base(strConn)
        {
        }

        TrVersionSp7Helper helper = new TrVersionSp7Helper();

        public int Save(TrVersionSp7DTO entity)
        {
            DbCommand command;
            string auditoria = entity.Verauditoria == null ? "N" : entity.Verauditoria;
            bool esauditoria = (auditoria == "S");

            if (ConstantesBase.EmprcodiSistema == entity.Emprcodieje && !esauditoria)
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
            
            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodieje, DbType.Int32, entity.Emprcodieje);
            dbProvider.AddInParameter(command, helper.Verfechaini, DbType.DateTime, entity.Verfechaini);
            dbProvider.AddInParameter(command, helper.Verfechafin, DbType.DateTime, entity.Verfechafin);
            dbProvider.AddInParameter(command, helper.Vernota, DbType.String, entity.Vernota);
            dbProvider.AddInParameter(command, helper.Vernotaeje, DbType.String, entity.Vernotaeje);
            dbProvider.AddInParameter(command, helper.Vernumero, DbType.Int32, entity.Vernumero);
            dbProvider.AddInParameter(command, helper.Verrepoficial, DbType.String, entity.Verrepoficial);
            dbProvider.AddInParameter(command, helper.Verestado, DbType.String, entity.Verestado);
            dbProvider.AddInParameter(command, helper.Verreprocestadcanal, DbType.String, entity.Verreprocestadcanal);
            dbProvider.AddInParameter(command, helper.Verconsidexclus, DbType.String, entity.Verconsidexclus);
            dbProvider.AddInParameter(command, helper.Verestadisticacontabmae, DbType.String, entity.Verestadisticacontabmae);
            dbProvider.AddInParameter(command, helper.Verestadisticamaefecha, DbType.DateTime, entity.Verestadisticamaefecha);
            dbProvider.AddInParameter(command, helper.Verauditoria, DbType.String, entity.Verauditoria);            
            dbProvider.AddInParameter(command, helper.Verultfechaejec, DbType.DateTime, entity.Verultfechaejec);
            dbProvider.AddInParameter(command, helper.Versionaplic, DbType.String, entity.Versionaplic);
            dbProvider.AddInParameter(command, helper.Verusucreacion, DbType.String, entity.Verusucreacion);
            dbProvider.AddInParameter(command, helper.Verfeccreacion, DbType.DateTime, entity.Verfeccreacion);
            dbProvider.AddInParameter(command, helper.Verusumodificacion, DbType.String, entity.Verusumodificacion);
            dbProvider.AddInParameter(command, helper.Verfecmodificacion, DbType.DateTime, entity.Verfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrVersionSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodieje, DbType.Int32, entity.Emprcodieje);
            dbProvider.AddInParameter(command, helper.Verfechaini, DbType.DateTime, entity.Verfechaini);
            dbProvider.AddInParameter(command, helper.Verfechafin, DbType.DateTime, entity.Verfechafin);
            dbProvider.AddInParameter(command, helper.Vernota, DbType.String, entity.Vernota);
            dbProvider.AddInParameter(command, helper.Vernotaeje, DbType.String, entity.Vernotaeje);
            dbProvider.AddInParameter(command, helper.Vernumero, DbType.Int32, entity.Vernumero);
            dbProvider.AddInParameter(command, helper.Verrepoficial, DbType.String, entity.Verrepoficial);
            dbProvider.AddInParameter(command, helper.Verestado, DbType.String, entity.Verestado);
            dbProvider.AddInParameter(command, helper.Verreprocestadcanal, DbType.String, entity.Verreprocestadcanal);
            dbProvider.AddInParameter(command, helper.Verconsidexclus, DbType.String, entity.Verconsidexclus);
            dbProvider.AddInParameter(command, helper.Verestadisticacontabmae, DbType.String, entity.Verestadisticacontabmae);
            dbProvider.AddInParameter(command, helper.Verestadisticamaefecha, DbType.DateTime, entity.Verestadisticamaefecha);
            dbProvider.AddInParameter(command, helper.Verauditoria, DbType.String, entity.Verauditoria);
            dbProvider.AddInParameter(command, helper.Verultfechaejec, DbType.DateTime, entity.Verultfechaejec);
            dbProvider.AddInParameter(command, helper.Versionaplic, DbType.String, entity.Versionaplic);
            dbProvider.AddInParameter(command, helper.Verusucreacion, DbType.String, entity.Verusucreacion);
            dbProvider.AddInParameter(command, helper.Verfeccreacion, DbType.DateTime, entity.Verfeccreacion);
            dbProvider.AddInParameter(command, helper.Verusumodificacion, DbType.String, entity.Verusumodificacion);
            dbProvider.AddInParameter(command, helper.Verfecmodificacion, DbType.DateTime, entity.Verfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, vercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrVersionSp7DTO GetById(int vercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, vercodi);
            TrVersionSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrVersionSp7DTO> List()
        {
            List<TrVersionSp7DTO> entitys = new List<TrVersionSp7DTO>();
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

        public List<TrVersionSp7DTO> List(DateTime verFecha)
        {                        
            List<TrVersionSp7DTO> entitys = new List<TrVersionSp7DTO>();
            String sql = String.Format(this.helper.SqlListFecha, verFecha.ToString(ConstantesBase.FormatoFecha));
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

        public List<TrVersionSp7DTO> ListPendiente()
        {
            List<TrVersionSp7DTO> entitys = new List<TrVersionSp7DTO>();
            String sql = String.Format(this.helper.SqlListPendiente);
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

        public List<TrVersionSp7DTO> GetByCriteria()
        {
            List<TrVersionSp7DTO> entitys = new List<TrVersionSp7DTO>();
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
        /// Graba los datos de la tabla TR_VERSION_SP7
        /// </summary>
        public int SaveTrVersionSp7Id(TrVersionSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Vercodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Vercodi;
                }

                return id;

            }
            catch (Exception ex)
            {                
                return 0;
            }
        }

        public List<TrVersionSp7DTO> BuscarOperaciones(DateTime verFechaini, DateTime verFechafin,  int nroPage, int pageSize)
        {
            List<TrVersionSp7DTO> entitys = new List<TrVersionSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, verFechaini.ToString(ConstantesBase.FormatoFecha),verFechafin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrVersionSp7DTO entity = new TrVersionSp7DTO();

                    int iVercodi = dr.GetOrdinal(this.helper.Vercodi);
                    if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

                    int iEmprcodieje = dr.GetOrdinal(this.helper.Emprcodieje);
                    if (!dr.IsDBNull(iEmprcodieje)) entity.Emprcodieje = Convert.ToInt32(dr.GetValue(iEmprcodieje));

                    int iEmprenomb = dr.GetOrdinal(this.helper.Emprenomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprenomb = dr.GetString(iEmprenomb);
                    
                    int iVerfechaini = dr.GetOrdinal(this.helper.Verfechaini);
                    if (!dr.IsDBNull(iVerfechaini)) entity.Verfechaini = dr.GetDateTime(iVerfechaini);

                    int iVerfechafin = dr.GetOrdinal(this.helper.Verfechafin);
                    if (!dr.IsDBNull(iVerfechafin)) entity.Verfechafin = dr.GetDateTime(iVerfechafin);

                    int iVernota = dr.GetOrdinal(this.helper.Vernota);
                    if (!dr.IsDBNull(iVernota)) entity.Vernota = dr.GetString(iVernota);

                    int iVernotaeje = dr.GetOrdinal(this.helper.Vernotaeje);
                    if (!dr.IsDBNull(iVernotaeje)) entity.Vernotaeje = dr.GetString(iVernotaeje);

                    int iVernumero = dr.GetOrdinal(this.helper.Vernumero);
                    if (!dr.IsDBNull(iVernumero)) entity.Vernumero = Convert.ToInt32(dr.GetValue(iVernumero));

                    int iVerrepoficial = dr.GetOrdinal(this.helper.Verrepoficial);
                    if (!dr.IsDBNull(iVerrepoficial)) entity.Verrepoficial = dr.GetString(iVerrepoficial);

                    int iVerestado = dr.GetOrdinal(this.helper.Verestado);
                    if (!dr.IsDBNull(iVerestado)) entity.Verestado = dr.GetString(iVerestado);

                    int iVerreprocestadcanal = dr.GetOrdinal(this.helper.Verreprocestadcanal);
                    if (!dr.IsDBNull(iVerreprocestadcanal)) entity.Verreprocestadcanal = dr.GetString(iVerreprocestadcanal);

                    int iVerconsidexclus = dr.GetOrdinal(this.helper.Verconsidexclus);
                    if (!dr.IsDBNull(iVerconsidexclus)) entity.Verconsidexclus = dr.GetString(iVerconsidexclus);

                    int iVerestadisticacontabmae = dr.GetOrdinal(this.helper.Verestadisticacontabmae);
                    if (!dr.IsDBNull(iVerestadisticacontabmae)) entity.Verestadisticacontabmae = dr.GetString(iVerestadisticacontabmae);

                    int iVerestadisticamaefecha = dr.GetOrdinal(this.helper.Verestadisticamaefecha);
                    if (!dr.IsDBNull(iVerestadisticamaefecha)) entity.Verestadisticamaefecha = dr.GetDateTime(iVerestadisticamaefecha);

                    int iVerauditoria = dr.GetOrdinal(this.helper.Verauditoria);
                    if (!dr.IsDBNull(iVerauditoria)) entity.Verauditoria = dr.GetString(iVerauditoria);

                    int iVerultfechaejec = dr.GetOrdinal(this.helper.Verultfechaejec);
                    if (!dr.IsDBNull(iVerultfechaejec)) entity.Verultfechaejec = dr.GetDateTime(iVerultfechaejec);

                    int iVersionaplic = dr.GetOrdinal(this.helper.Versionaplic);
                    if (!dr.IsDBNull(iVersionaplic)) entity.Versionaplic = dr.GetString(iVersionaplic);

                    int iVerusucreacion = dr.GetOrdinal(this.helper.Verusucreacion);
                    if (!dr.IsDBNull(iVerusucreacion)) entity.Verusucreacion = dr.GetString(iVerusucreacion);

                    int iVerfeccreacion = dr.GetOrdinal(this.helper.Verfeccreacion);
                    if (!dr.IsDBNull(iVerfeccreacion)) entity.Verfeccreacion = dr.GetDateTime(iVerfeccreacion);

                    int iVerusumodificacion = dr.GetOrdinal(this.helper.Verusumodificacion);
                    if (!dr.IsDBNull(iVerusumodificacion)) entity.Verusumodificacion = dr.GetString(iVerusumodificacion);

                    int iVerfecmodificacion = dr.GetOrdinal(this.helper.Verfecmodificacion);
                    if (!dr.IsDBNull(iVerfecmodificacion)) entity.Verfecmodificacion = dr.GetDateTime(iVerfecmodificacion);

                    int iVerestadodescrip = dr.GetOrdinal(this.helper.Verestadodescrip);
                    if (!dr.IsDBNull(iVerestadodescrip)) entity.Verestadodescrip = dr.GetString(iVerestadodescrip);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime verFechaini,DateTime verFechafin)
        {
            String sql = String.Format(this.helper.TotalRegistros, verFechaini.ToString(ConstantesBase.FormatoFecha),verFechafin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public int GetVersion(DateTime verFecha)
        {
            String sql = String.Format(this.helper.SqlGetVersion, verFecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

    }
}

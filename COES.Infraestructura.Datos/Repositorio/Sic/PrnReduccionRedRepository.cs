using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnReduccionRedRepository : RepositoryBase, IPrnReduccionRedRepository
    {
        public PrnReduccionRedRepository(string strConn)
         : base(strConn)
        {
        }

        PrnReduccionRedHelper helper = new PrnReduccionRedHelper();

        public List<PrnReduccionRedDTO> ListByNombre()
        {
            List<PrnReduccionRedDTO> entitys = new List<PrnReduccionRedDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByNombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    int iPrnredbarracp = dr.GetOrdinal(helper.Prnredbarracp);
                    if (!dr.IsDBNull(iPrnredbarracp)) entity.Prnredbarracp = Convert.ToInt32(dr.GetValue(iPrnredbarracp));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnReduccionRedDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prnredcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, entity.Prnvercodi);
            dbProvider.AddInParameter(command, helper.Prnredbarracp, DbType.Int32, entity.Prnredbarracp);
            dbProvider.AddInParameter(command, helper.Prnredbarrapm, DbType.Int32, entity.Prnredbarrapm);
            dbProvider.AddInParameter(command, helper.Prnredgauss, DbType.Decimal, entity.Prnredgauss);
            dbProvider.AddInParameter(command, helper.Prnredperdida, DbType.Decimal, entity.Prnredperdida);
            dbProvider.AddInParameter(command, helper.Prnredfecha, DbType.DateTime, entity.Prnredfecha);
            dbProvider.AddInParameter(command, helper.Prnredusucreacion, DbType.String, entity.Prnredusucreacion);
            dbProvider.AddInParameter(command, helper.Prnredfeccreacion, DbType.DateTime, entity.Prnredfeccreacion);
            dbProvider.AddInParameter(command, helper.Prnredusumodificacion, DbType.String, entity.Prnredusumodificacion);
            dbProvider.AddInParameter(command, helper.Prnredfecmodificacion, DbType.DateTime, entity.Prnredfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prnrednombre, DbType.String, entity.Prnrednombre);
            dbProvider.AddInParameter(command, helper.Prnredtipo, DbType.String, entity.Prnredtipo);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnReduccionRedDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, entity.Prnvercodi);
            dbProvider.AddInParameter(command, helper.Prnredbarracp, DbType.Int32, entity.Prnredbarracp);
            dbProvider.AddInParameter(command, helper.Prnredbarrapm, DbType.Int32, entity.Prnredbarrapm);
            dbProvider.AddInParameter(command, helper.Prnredgauss, DbType.Decimal, entity.Prnredgauss);
            dbProvider.AddInParameter(command, helper.Prnredperdida, DbType.Decimal, entity.Prnredperdida);
            dbProvider.AddInParameter(command, helper.Prnredfecha, DbType.DateTime, entity.Prnredfecha);
            dbProvider.AddInParameter(command, helper.Prnredusumodificacion, DbType.String, entity.Prnredusumodificacion);
            dbProvider.AddInParameter(command, helper.Prnredfecmodificacion, DbType.DateTime, entity.Prnredfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prnrednombre, DbType.String, entity.Prnrednombre);
            dbProvider.AddInParameter(command, helper.Prnredtipo, DbType.String, entity.Prnredtipo);
            dbProvider.AddInParameter(command, helper.Prnredcodi, DbType.Int32, entity.Prnredcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnReduccionRedDTO> ListByCPNivel(int version, string prnredtipo)
        {
            List<PrnReduccionRedDTO> entitys = new List<PrnReduccionRedDTO>();

            string query = string.Format(helper.SqlListByCPNivel, version, prnredtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    int iPrnredcodi = dr.GetOrdinal(helper.Prnredcodi);
                    if (!dr.IsDBNull(iPrnredcodi)) entity.Prnredcodi = Convert.ToInt32(dr.GetValue(iPrnredcodi));

                    int iPrnrednombre = dr.GetOrdinal(helper.Prnrednombre);
                    if (!dr.IsDBNull(iPrnrednombre)) entity.Prnrednombre = dr.GetString(iPrnrednombre);

                    int iPrnredbarracp = dr.GetOrdinal(helper.Prnredbarracp);
                    if (!dr.IsDBNull(iPrnredbarracp)) entity.Prnredbarracp = Convert.ToInt32(dr.GetValue(iPrnredbarracp));

                    int iNombrecp = dr.GetOrdinal(helper.Nombrecp);
                    if (!dr.IsDBNull(iNombrecp)) entity.Nombrecp = dr.GetString(iNombrecp);

                    int iPrnredbarrapm = dr.GetOrdinal(helper.Prnredbarrapm);
                    if (!dr.IsDBNull(iPrnredbarrapm)) entity.Prnredbarrapm = Convert.ToInt32(dr.GetValue(iPrnredbarrapm));

                    int iNombrepm = dr.GetOrdinal(helper.Nombrepm);
                    if (!dr.IsDBNull(iNombrepm)) entity.Nombrepm = dr.GetString(iNombrepm);

                    int iPrnredgauss = dr.GetOrdinal(helper.Prnredgauss);
                    if (!dr.IsDBNull(iPrnredgauss)) entity.Prnredgauss = dr.GetDecimal(iPrnredgauss);

                    int iPrnredperdida = dr.GetOrdinal(helper.Prnredperdida);
                    if (!dr.IsDBNull(iPrnredperdida)) entity.Prnredperdida = dr.GetDecimal(iPrnredperdida);

                    int iPrnredtipo = dr.GetOrdinal(helper.Prnredtipo);
                    if (!dr.IsDBNull(iPrnredtipo)) entity.Prnredtipo = dr.GetString(iPrnredtipo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnReduccionRedDTO> ListPuntosAgrupacionesByBarra(string prnredtipo)
        {
            List<PrnReduccionRedDTO> entitys = new List<PrnReduccionRedDTO>();

            string query = string.Format(helper.SqlListPuntosAgrupacionesByBarra, prnredtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iNombre = dr.GetOrdinal(helper.Nombre);
                    if (!dr.IsDBNull(iNombre)) entity.Nombre = dr.GetString(iNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteReduccionRed(int reduccionred, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteReduccionRed);

            dbProvider.AddInParameter(command, helper.Prnredcodi, DbType.Int32, reduccionred);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, version);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnReduccionRedDTO> GetModeloActivo()
        {
            List<PrnReduccionRedDTO> entitys = new List<PrnReduccionRedDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetModeloActivo);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();
                    
                    int iPrnredbarracp = dr.GetOrdinal(helper.Prnredbarracp);
                    if (!dr.IsDBNull(iPrnredbarracp)) entity.Prnredbarracp = Convert.ToInt32(dr.GetValue(iPrnredbarracp));
                    
                    int iPrnredbarrapm = dr.GetOrdinal(helper.Prnredbarrapm);
                    if (!dr.IsDBNull(iPrnredbarrapm)) entity.Prnredbarrapm = Convert.ToInt32(dr.GetValue(iPrnredbarrapm));
                    
                    int iPrnredgauss = dr.GetOrdinal(helper.Prnredgauss);
                    if (!dr.IsDBNull(iPrnredgauss)) entity.Prnredgauss = dr.GetDecimal(iPrnredgauss);

                    int iPrnredperdida = dr.GetOrdinal(helper.Prnredperdida);
                    if (!dr.IsDBNull(iPrnredperdida)) entity.Prnredperdida = dr.GetDecimal(iPrnredperdida);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeletePrnReduccionRedBarraVersion(int barrapm, int barracp, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteReduccionRedBarraVersion);

            dbProvider.AddInParameter(command, helper.Prnredcodi, DbType.Int32, barrapm);
            dbProvider.AddInParameter(command, helper.Prnredcodi, DbType.Int32, barracp);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, version);
            dbProvider.ExecuteNonQuery(command);
        }

        //18032020
        
        public List<PrnReduccionRedDTO> ListSumaBarraGaussPM(int version, string barraspm)
        {
            List<PrnReduccionRedDTO> entitys = new List<PrnReduccionRedDTO>();
            string query = string.Format(helper.SqlListSumaBarraGaussPM, version, barraspm);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    int iPrnredbarrapm = dr.GetOrdinal(helper.Prnredbarrapm);
                    if (!dr.IsDBNull(iPrnredbarrapm)) entity.Prnredbarrapm = Convert.ToInt32(dr.GetValue(iPrnredbarrapm));

                    int iPrnredgauss = dr.GetOrdinal(helper.Prnredgauss);
                    if (!dr.IsDBNull(iPrnredgauss)) entity.Prnredgauss = dr.GetDecimal(iPrnredgauss);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnReduccionRedDTO> ListBarraCPPorArea(int areapadre)
        {
            List<PrnReduccionRedDTO> entitys = new List<PrnReduccionRedDTO>();
            string query = string.Format(helper.SqlListBarraCPPorArea, areapadre);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    int iPrnredbarracp = dr.GetOrdinal(helper.Prnredbarracp);
                    if (!dr.IsDBNull(iPrnredbarracp)) entity.Prnredbarracp = Convert.ToInt32(dr.GetValue(iPrnredbarracp));
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

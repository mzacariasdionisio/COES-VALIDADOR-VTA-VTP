using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnPtomedpropRepository : RepositoryBase, IPrnPtomedpropRepository
    {
        public PrnPtomedpropRepository(string strConn) : base(strConn)
        {
        }

        PrnPtomedpropHelper helper = new PrnPtomedpropHelper();

        public void Save(PrnPtomedpropDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnpmpvarexoproceso, DbType.String, entity.Prnpmpvarexoproceso);
            dbProvider.AddInParameter(command, helper.Prnpmpusucreacion, DbType.String, entity.Prnpmpusucreacion);
            dbProvider.AddInParameter(command, helper.Prnpmpfeccreacion, DbType.DateTime, entity.Prnpmpfeccreacion);
            dbProvider.AddInParameter(command, helper.Prnpmpusucreacion, DbType.String, entity.Prnpmpusucreacion);
            dbProvider.AddInParameter(command, helper.Prnpmpfecmodificacion, DbType.DateTime, entity.Prnpmpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnPtomedpropDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Prnpmpvarexoproceso, DbType.String, entity.Prnpmpvarexoproceso);
            dbProvider.AddInParameter(command, helper.Prnpmpusucreacion, DbType.String, entity.Prnpmpusucreacion);
            dbProvider.AddInParameter(command, helper.Prnpmpfecmodificacion, DbType.DateTime, entity.Prnpmpfecmodificacion);
            
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnPtomedpropDTO> List()
        {
            List<PrnPtomedpropDTO> entitys = new List<PrnPtomedpropDTO>();
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

        public PrnPtomedpropDTO GetById(int ptomedicodi)
        {
            PrnPtomedpropDTO entity = new PrnPtomedpropDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MePtomedicionDTO> PR03Puntos()
        {
            PrnMedicion48Helper M48Helper = new PrnMedicion48Helper();

            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlPR03Puntos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iAreaOperativa = dr.GetOrdinal(M48Helper.AreaOperativa);
                    if (!dr.IsDBNull(iAreaOperativa)) entity.AreaOperativa = dr.GetString(iAreaOperativa);

                    int iTareaabrev = dr.GetOrdinal(M48Helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreacodi = dr.GetOrdinal(M48Helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(M48Helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTipoemprcodi = dr.GetOrdinal(M48Helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(M48Helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(M48Helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(M48Helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(M48Helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(M48Helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(M48Helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(M48Helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(M48Helper.Grupocodibarra);
                    if (!dr.IsDBNull(iAreacodi)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iPrnpmpvarexoproceso = dr.GetOrdinal(helper.Prnpmpvarexoproceso);
                    if (!dr.IsDBNull(iPrnpmpvarexoproceso)) entity.Prnpmpvarexoproceso = dr.GetString(iPrnpmpvarexoproceso);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnAgrupacionDTO> PR03Agrupaciones(int origlectcodi, int grupocodibarra)
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            string query = string.Format(helper.SqlPR03Agrupaciones, origlectcodi, grupocodibarra);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnAgrupacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodibarra = dr.GetOrdinal(helper.Grupocodibarra);
                    if (!dr.IsDBNull(iGrupocodibarra)) entity.Grupocodibarra = Convert.ToInt32(dr.GetValue(iGrupocodibarra));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtogrphijocodi = dr.GetOrdinal(helper.Ptogrphijocodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptogrphijocodi = Convert.ToInt32(dr.GetValue(iPtogrphijocodi));

                    int iPtogrphijodesc = dr.GetOrdinal(helper.Ptogrphijodesc);
                    if (!dr.IsDBNull(iPtogrphijodesc)) entity.Ptogrphijodesc = dr.GetString(iPtogrphijodesc);

                    int iPrnpmpvarexoproceso = dr.GetOrdinal(helper.Prnpmpvarexoproceso);
                    if (!dr.IsDBNull(iPrnpmpvarexoproceso)) entity.Prnpmpvarexoproceso = dr.GetString(iPrnpmpvarexoproceso);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

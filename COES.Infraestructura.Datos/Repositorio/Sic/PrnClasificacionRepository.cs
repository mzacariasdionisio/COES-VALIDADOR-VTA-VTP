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
    public class PrnClasificacionRepository : RepositoryBase, IPrnClasificacionRepository
    {
        public PrnClasificacionRepository(string strConn)
            : base(strConn)
        {
        }

        PrnClasificacionHelper helper = new PrnClasificacionHelper();

        public void Save(PrnClasificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prnclscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);            
            dbProvider.AddInParameter(command, helper.Prnclsfecha, DbType.DateTime, entity.Prnclsfecha);
            dbProvider.AddInParameter(command, helper.Prnclsclasificacion, DbType.Int32, entity.Prnclsclasificacion);
            dbProvider.AddInParameter(command, helper.Prnclsporcerrormin, DbType.Decimal, entity.Prnclsporcerrormin);
            dbProvider.AddInParameter(command, helper.Prnclsporcerrormax, DbType.Decimal, entity.Prnclsporcerrormax);
            dbProvider.AddInParameter(command, helper.Prnclsmagcargamin, DbType.Decimal, entity.Prnclsmagcargamin);
            dbProvider.AddInParameter(command, helper.Prnclsmagcargamax, DbType.Decimal, entity.Prnclsmagcargamax);
            dbProvider.AddInParameter(command, helper.Prnclsestado, DbType.String, entity.Prnclsestado);
            dbProvider.AddInParameter(command, helper.Prnclsperfil, DbType.Int32, entity.Prnclsperfil);
            dbProvider.AddInParameter(command, helper.Prnclsvariacion, DbType.Decimal, entity.Prnclsvariacion);
            dbProvider.AddInParameter(command, helper.Prnclsusucreacion, DbType.String, entity.Prnclsusucreacion);
            dbProvider.AddInParameter(command, helper.Prnclsfeccreacion, DbType.DateTime, entity.Prnclsfeccreacion);
            dbProvider.AddInParameter(command, helper.Prnclsusumodificacion, DbType.String, entity.Prnclsusumodificacion);
            dbProvider.AddInParameter(command, helper.Prnclsfecmodificacion, DbType.DateTime, entity.Prnclsfecmodificacion);

            dbProvider.ExecuteNonQuery(command);            
        }

        public void Update(PrnClasificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Prnclsclasificacion, DbType.Int32, entity.Prnclsclasificacion);
            dbProvider.AddInParameter(command, helper.Prnclsporcerrormin, DbType.Decimal, entity.Prnclsporcerrormin);
            dbProvider.AddInParameter(command, helper.Prnclsporcerrormax, DbType.Decimal, entity.Prnclsporcerrormax);
            dbProvider.AddInParameter(command, helper.Prnclsmagcargamin, DbType.String, entity.Prnclsmagcargamin);
            dbProvider.AddInParameter(command, helper.Prnclsmagcargamax, DbType.String, entity.Prnclsmagcargamax);
            dbProvider.AddInParameter(command, helper.Prnclsestado, DbType.String, entity.Prnclsestado);
            dbProvider.AddInParameter(command, helper.Prnclsperfil, DbType.Int32, entity.Prnclsperfil);
            dbProvider.AddInParameter(command, helper.Prnclsvariacion, DbType.Decimal, entity.Prnclsvariacion);
            dbProvider.AddInParameter(command, helper.Prnclsusumodificacion, DbType.String, entity.Prnclsusumodificacion);
            dbProvider.AddInParameter(command, helper.Prnclsfecmodificacion, DbType.DateTime, entity.Prnclsfecmodificacion);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);            
            dbProvider.AddInParameter(command, helper.Prnclsfecha, DbType.DateTime, entity.Prnclsfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi, int lectcodi, DateTime prnclsfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Prnclsfecha, DbType.DateTime, prnclsfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrnClasificacionDTO GetById(int ptomedicodi, int lectcodi, DateTime prnclsfecha)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Prnclsfecha, DbType.DateTime, prnclsfecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnClasificacionDTO> List()
        {
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
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

        public List<PrnClasificacionDTO> ListClasificacion48(DateTime medifecha, int prnmtipo, int lectcodi, int anivelcodi)//ELIMINAR??
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListClasificacion48);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Prnm48tipo, DbType.Int32, prnmtipo);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iPrnclsestado = dr.GetOrdinal(helper.Prnclsestado);
                    if (!dr.IsDBNull(iPrnclsestado)) entity.Prnclsestado = dr.GetString(iPrnclsestado);

                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnClasificacionDTO> ListClasificacion96(DateTime medifecha, int prnmtipo, int lectcodi, int anivelcodi)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListClasificacion96);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Prnm96tipo, DbType.Int32, prnmtipo);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iPrnclsestado = dr.GetOrdinal(helper.Prnclsestado);
                    if (!dr.IsDBNull(iPrnclsestado)) entity.Prnclsestado = dr.GetString(iPrnclsestado);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }//ELIMINAR?

        public List<PrnClasificacionDTO> ListProdemPuntos(string areaoperativa)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListProdemPuntos);
            dbProvider.AddInParameter(command, helper.Areaoperativa, DbType.String, areaoperativa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entity.FullPtomedidesc = entity.Areanomb + " " + entity.Ptomedidesc;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnClasificacionDTO> ListPuntosClasificados48(DateTime medifecha)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntosClasificados48);
            dbProvider.AddInParameter(command, helper.Prnclsfecha, DbType.DateTime, medifecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iPrnclsestado = dr.GetOrdinal(helper.Prnclsestado);
                    if (!dr.IsDBNull(iPrnclsestado)) entity.Prnclsestado = dr.GetString(iPrnclsestado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnClasificacionDTO> ListPuntosClasificados96(DateTime medifecha)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntosClasificados96);
            dbProvider.AddInParameter(command, helper.Prnclsfecha, DbType.DateTime, medifecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iPrnclsestado = dr.GetOrdinal(helper.Prnclsestado);
                    if (!dr.IsDBNull(iPrnclsestado)) entity.Prnclsestado = dr.GetString(iPrnclsestado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CountMedicionesByRangoFechas(int ptomedicodi, int prnmtipo, DateTime fecini, DateTime fecfin)
        {
            int total = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCountMedicionesByRangoFechas);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnm48tipo, DbType.Int32, prnmtipo);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fecini);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fecfin);
            dbProvider.AddInParameter(command, helper.Prnm96tipo, DbType.Int32, prnmtipo);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    total = dr.GetInt32(0);
                }
                
            }

            return total;                
        }

        //Inicio PRODEM 2 - Nuevas implementaciones 20190512
        public List<PrnClasificacionDTO> ListDemandaClasificada(string ptomedicodi, string medifecha, string prnm48tipo,
            int lectcodi, int tipoinfocodi, int tipoemprcodi, string areacodi, string emprcodi, string prnclsperfil, 
            string prnclsclasificacion, string fechafin, string aocodi, string order)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            string query = String.Format(helper.SqlListDemandaClasificada, ptomedicodi, medifecha, prnm48tipo, lectcodi, 
                tipoinfocodi, tipoemprcodi, areacodi, emprcodi, prnclsperfil, prnclsclasificacion, fechafin, aocodi, order);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();
                    
                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iPrnclsperfil = dr.GetOrdinal(helper.Prnclsperfil);
                    if (!dr.IsDBNull(iPrnclsperfil)) entity.Prnclsperfil = Convert.ToInt32(dr.GetValue(iPrnclsperfil));

                    int iPrnclsvariacion = dr.GetOrdinal(helper.Prnclsvariacion);
                    if (!dr.IsDBNull(iPrnclsvariacion)) entity.Prnclsvariacion = dr.GetDecimal(iPrnclsvariacion);

                    entity.StrSubcausacodi = string.Empty;
                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.StrSubcausacodi = dr.GetString(iSubcausacodi);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);
                    
                    entity.FullPtomedidesc = entity.Areanomb + " " + entity.Ptomedidesc;

                    entity.ListSubcausacodi = entity.StrSubcausacodi.Split(',').ToList() ?? new List<string>();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnClasificacionDTO> ListDemandaClasificadaBarrasCP(string ptomedicodi, string medifecha, string prnm48tipo,
            int lectcodi, int tipoinfocodi, int tipoemprcodi, string areacodi, string emprcodi, string prnclsperfil,
            string prnclsclasificacion, string fechafin, string aocodi, string order)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            string query = String.Format(helper.SqlListDemandaClasificadaBarrasCP, ptomedicodi, medifecha, prnm48tipo, lectcodi,
                tipoinfocodi, tipoemprcodi, areacodi, emprcodi, prnclsperfil, prnclsclasificacion, fechafin, aocodi, order);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iPrnclsclasificacion = dr.GetOrdinal(helper.Prnclsclasificacion);
                    if (!dr.IsDBNull(iPrnclsclasificacion)) entity.Prnclsclasificacion = Convert.ToInt32(dr.GetValue(iPrnclsclasificacion));

                    int iPrnclsperfil = dr.GetOrdinal(helper.Prnclsperfil);
                    if (!dr.IsDBNull(iPrnclsperfil)) entity.Prnclsperfil = Convert.ToInt32(dr.GetValue(iPrnclsperfil));

                    int iPrnclsvariacion = dr.GetOrdinal(helper.Prnclsvariacion);
                    if (!dr.IsDBNull(iPrnclsvariacion)) entity.Prnclsvariacion = dr.GetDecimal(iPrnclsvariacion);

                    entity.StrSubcausacodi = string.Empty;
                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.StrSubcausacodi = dr.GetString(iSubcausacodi);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entity.FullPtomedidesc = entity.Areanomb + " " + entity.Ptomedidesc;

                    entity.ListSubcausacodi = entity.StrSubcausacodi.Split(',').ToList() ?? new List<string>();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnClasificacionDTO> ListDemandaNoClasificada(string ptomedicodi, string medifecha, string prnm48tipo,
            int lectcodi, int tipoinfocodi, int tipoemprcodi, string areacodi, string emprcodi)
        {
            PrnClasificacionDTO entity = new PrnClasificacionDTO();
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            string query = String.Format(helper.SqlListDemandaNoClasificada, ptomedicodi, medifecha, prnm48tipo, lectcodi,
                tipoinfocodi, tipoemprcodi, areacodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnClasificacionDTO();

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iPrnmestado)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    entity.Prnflgclasificacion = false;

                    entity.FullPtomedidesc = entity.Areanomb + " " + entity.Ptomedidesc;
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Fin PRODEM 2 - Nuevas implementaciones 20190512
    }
}

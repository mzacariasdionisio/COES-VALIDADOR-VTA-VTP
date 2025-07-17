using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnAgrupacionRepository : RepositoryBase, IPrnAgrupacionRepository
    {
        public PrnAgrupacionRepository(string strConn)
            : base(strConn)
        {
        }

        PrnAgrupacionHelper helper = new PrnAgrupacionHelper();
        MePtomedicionHelper helperPtomedicion = new MePtomedicionHelper();

        public List<PrnAgrupacionDTO> List()
        {
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
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

        public void Save(PrnAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Agrupcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptogrpcodi, DbType.Int32, entity.Ptogrpcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Agrupfactor, DbType.Int32, entity.Agrupfactor);
            dbProvider.AddInParameter(command, helper.Agrupfechaini, DbType.DateTime, entity.Agrupfechaini);
            dbProvider.AddInParameter(command, helper.Agrupfechafin, DbType.DateTime, entity.Agrupfechafin);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnAgrupacionDTO> ListById(int ptogrpcodi)
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListById);
            dbProvider.AddInParameter(command, helper.Ptogrpcodi, DbType.Int32, ptogrpcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnAgrupacionDTO();

                    int iAgrupcodi = dr.GetOrdinal(helper.Agrupcodi);
                    if (!dr.IsDBNull(iAgrupcodi)) entity.Agrupcodi = Convert.ToInt32(dr.GetValue(iAgrupcodi));

                    int iPtogrpcodi = dr.GetOrdinal(helper.Ptogrpcodi);
                    if (!dr.IsDBNull(iPtogrpcodi)) entity.Ptogrpcodi = Convert.ToInt32(dr.GetValue(iPtogrpcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helperPtomedicion.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iAgrupfactor = dr.GetOrdinal(helper.Agrupfactor);
                    if (!dr.IsDBNull(iAgrupfactor)) entity.Agrupfactor = Convert.ToInt32(dr.GetValue(iAgrupfactor));

                    int iAgrupfechaini = dr.GetOrdinal(helper.Agrupfechaini);
                    if (!dr.IsDBNull(iAgrupfechaini)) entity.Agrupfechaini = dr.GetDateTime(iAgrupfechaini);

                    int iAgrupfechafin = dr.GetOrdinal(helper.Agrupfechafin);
                    if (!dr.IsDBNull(iAgrupfechafin)) entity.Agrupfechafin = dr.GetDateTime(iAgrupfechafin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public List<MePtomedicionDTO> ListMeAgrupacion()
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMeAgrupacion);            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helperPtomedicion.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helperPtomedicion.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iAreacodi = dr.GetOrdinal(helperPtomedicion.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helperPtomedicion.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprcodi = dr.GetOrdinal(helperPtomedicion.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helperPtomedicion.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtogrpcodi = dr.GetOrdinal(helper.Ptogrpcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Ptogrpcodi = Convert.ToInt32(dr.GetValue(iPtogrpcodi));

                    int iPtogrppronostico = dr.GetOrdinal(helper.Ptogrppronostico);
                    if (!dr.IsDBNull(iPtogrppronostico)) entity.Ptogrppronostico = Convert.ToInt32(dr.GetValue(iPtogrppronostico));

                    int iPtogrpfechaini = dr.GetOrdinal(helper.Ptogrpfechaini);
                    if (!dr.IsDBNull(iPtogrpfechaini)) entity.Ptogrpfechaini = dr.GetDateTime(iPtogrpfechaini);

                    int iPtogrpfechafin = dr.GetOrdinal(helper.Ptogrpfechafin);
                    if (!dr.IsDBNull(iPtogrpfechafin)) entity.Ptogrpfechafin = dr.GetDateTime(iPtogrpfechafin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListAgrupacionesActivas(string areacodi, string ptomedicodi, string emprcodi, int esPronostico)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string query = string.Format(helper.SqlListAgrupacionesActivas, areacodi, ptomedicodi, emprcodi, esPronostico);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();
                    
                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtogrpcodi = dr.GetOrdinal(helper.Ptogrpcodi);
                    if (!dr.IsDBNull(iPtogrpcodi)) entity.Ptogrpcodi = Convert.ToInt32(dr.GetValue(iPtogrpcodi));

                    int iPtogrppronostico = dr.GetOrdinal(helper.Ptogrppronostico);
                    if (!dr.IsDBNull(iPtogrppronostico)) entity.Ptogrppronostico = Convert.ToInt32(dr.GetValue(iPtogrppronostico));

                    int iPtogrphijocodi = dr.GetOrdinal(helper.Ptogrphijocodi);
                    if (!dr.IsDBNull(iPtogrphijocodi)) entity.Ptogrphijocodi = Convert.ToInt32(dr.GetValue(iPtogrphijocodi));

                    int iPtogrphijodesc = dr.GetOrdinal(helper.Ptogrphijodesc);
                    if (!dr.IsDBNull(iPtogrphijodesc)) entity.Ptogrphijodesc = dr.GetString(iPtogrphijodesc);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int SavePuntoAgrupacion(PrnPuntoAgrupacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdPuntoAgrupacion);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSavePuntoAgrupacion);

            dbProvider.AddInParameter(command, helper.Ptogrpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptogrppronostico, DbType.Int32, entity.Ptogrppronostico);
            dbProvider.AddInParameter(command, helper.Ptogrpfechaini, DbType.DateTime, entity.Ptogrpfechaini);
            dbProvider.AddInParameter(command, helper.Ptogrpfechafin, DbType.DateTime, entity.Ptogrpfechafin);
            dbProvider.AddInParameter(command, helper.Ptogrpusumodificacion, DbType.String, entity.Ptogrpusumodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<PrnPuntoAgrupacionDTO> ListByIdPuntoAgrupacion(int ptomedicodi)
        {
            PrnPuntoAgrupacionDTO entity = new PrnPuntoAgrupacionDTO();
            List<PrnPuntoAgrupacionDTO> entitys = new List<PrnPuntoAgrupacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByIdPuntoAgrupacion);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnPuntoAgrupacionDTO();

                    int iPtogrpcodi = dr.GetOrdinal(helper.Ptogrpcodi);
                    if (!dr.IsDBNull(iPtogrpcodi)) entity.Ptogrpcodi = Convert.ToInt32(dr.GetValue(iPtogrpcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtogrpfechaini = dr.GetOrdinal(helper.Ptogrpfechaini);
                    if (!dr.IsDBNull(iPtogrpfechaini)) entity.Ptogrpfechaini = dr.GetDateTime(iPtogrpfechaini);

                    int iPtogrpfechafin = dr.GetOrdinal(helper.Ptogrpfechafin);
                    if (!dr.IsDBNull(iPtogrpfechafin)) entity.Ptogrpfechafin = dr.GetDateTime(iPtogrpfechafin);

                    int iPtogrppronostico = dr.GetOrdinal(helper.Ptogrppronostico);
                    if (!dr.IsDBNull(iPtogrppronostico)) entity.Ptogrppronostico = Convert.ToInt32(dr.GetValue(iPtogrppronostico));

                    int iPtogrpusumodificacion = dr.GetOrdinal(helper.Ptogrpusumodificacion);
                    if (!dr.IsDBNull(iPtogrpusumodificacion)) entity.Ptogrpusumodificacion = dr.GetString(iPtogrpusumodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<PrnAgrupacionDTO> ListPtosAgrupadosParaProdem()
        {
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPtosAgrupadosParaProdem);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void CerrarPuntoAgrupacion(int ptogrpcodi, DateTime ptogrpfechafin)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCerrarPuntoAgrupacion);
            dbProvider.AddInParameter(command, helper.Agrupfechafin, DbType.DateTime, ptogrpfechafin);
            dbProvider.AddInParameter(command, helper.Ptogrpcodi, DbType.Int32, ptogrpcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int ValidarNombreAgrupacion(string ptomedidesc)
        {
            int i = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarNombreAgrupacion);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, ptomedidesc);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iValido = dr.GetOrdinal(helper.Valido);
                    if (!dr.IsDBNull(iValido)) i = Convert.ToInt32(dr.GetValue(iValido));
                }
            }

            return i;
        }

        public List<PrnAgrupacionDTO> ListDemandaAgrupada(string areacodi, string emprcodi, string ptomedicodi)
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            string query = string.Format(helper.SqlListDemandaAgrupada, areacodi, emprcodi, ptomedicodi);
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

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtogrppronostico = dr.GetOrdinal(helper.Ptogrppronostico);
                    if (!dr.IsDBNull(iPtogrppronostico)) entity.Ptogrppronostico = Convert.ToInt32(dr.GetValue(iPtogrppronostico));

                    int iPtogrphijocodi = dr.GetOrdinal(helper.Ptogrphijocodi);
                    if (!dr.IsDBNull(iPtogrphijocodi)) entity.Ptogrphijocodi = Convert.ToInt32(dr.GetValue(iPtogrphijocodi));

                    int iPtogrphijodesc = dr.GetOrdinal(helper.Ptogrphijodesc);
                    if (!dr.IsDBNull(iPtogrphijodesc)) entity.Ptogrphijodesc = dr.GetString(iPtogrphijodesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnAgrupacionDTO> ListDemandaAgrupada(int origlectcodi, int origlectcodi2, int lectcodi, 
            int tipoinfocodi, string medifecha, string prnm48tipo, string areacodi, string emprcodi,
            string ptomedicodi, int ptogrppronostico, int prnm48tipo2)
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();

            string query = string.Format(helper.SqlListDemandaAgrupada, origlectcodi, origlectcodi2, lectcodi,
                tipoinfocodi, medifecha, prnm48tipo, areacodi, emprcodi, ptomedicodi, ptogrppronostico, prnm48tipo2);
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

                    int iPrnm48tipo = dr.GetOrdinal(helper.Prnm48tipo);
                    if (!dr.IsDBNull(iPrnm48tipo)) entity.Prnm48tipo = Convert.ToInt32(dr.GetValue(iPrnm48tipo));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtogrppronostico = dr.GetOrdinal(helper.Ptogrppronostico);
                    if (!dr.IsDBNull(iPtogrppronostico)) entity.Ptogrppronostico = Convert.ToInt32(dr.GetValue(iPtogrppronostico));

                    int iPtogrphijocodi = dr.GetOrdinal(helper.Ptogrphijocodi);
                    if (!dr.IsDBNull(iPtogrphijocodi)) entity.Ptogrphijocodi = Convert.ToInt32(dr.GetValue(iPtogrphijocodi));

                    int iPtogrphijodesc = dr.GetOrdinal(helper.Ptogrphijodesc);
                    if (!dr.IsDBNull(iPtogrphijodesc)) entity.Ptogrphijodesc = dr.GetString(iPtogrphijodesc);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iPrnmestado = dr.GetOrdinal(helper.Prnmestado);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Prnmestado = Convert.ToInt32(dr.GetValue(iPrnmestado));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedicionDTO> ListPuntosPR03(string aonomb, string tipoemprcodi, string areacodi, string emprcodi, string ptomedicodi)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            string query = string.Format(helper.SqlListPuntosPR03, aonomb, tipoemprcodi, areacodi, emprcodi, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqAreaDTO> ListUbicacionesPR03(string aonomb)
        {
            EqAreaDTO entity = new EqAreaDTO();
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            string query = string.Format(helper.SqlListUbicacionesPR03, aonomb);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EqAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListEmpresasPR03(string tipoemprcodi, string emprcodi)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListEmpresasPR03, tipoemprcodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnAgrupacionDTO> ListPuntosSeleccionados()
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntosSeleccionados);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnAgrupacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iAgrupfactor = dr.GetOrdinal(helper.Agrupfactor);
                    if (!dr.IsDBNull(iAgrupfactor)) entity.Agrupfactor = Convert.ToInt32(dr.GetValue(iAgrupfactor));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnAgrupacionDTO> GetDetalleAgrupacion(int ptomedicodi)
        {
            PrnAgrupacionDTO entity = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> entitys = new List<PrnAgrupacionDTO>();

            string query = string.Format(helper.SqlGetDetalleAgrupacion, ptomedicodi);
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

                    int iAgrupfactor = dr.GetOrdinal(helper.Agrupfactor);
                    if (!dr.IsDBNull(iAgrupfactor)) entity.Agrupfactor = Convert.ToInt32(dr.GetValue(iAgrupfactor));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public MePtomedicionDTO GetAgrupacion(int ptomedicodi)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();

            string query = string.Format(helper.SqlGetAgrupacion, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtogrppronostico = dr.GetOrdinal(helper.Ptogrppronostico);
                    if (!dr.IsDBNull(iPtogrppronostico)) entity.Ptogrppronostico = Convert.ToInt32(dr.GetValue(iPtogrppronostico));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                }
            }

            return entity;
        }
    }
}

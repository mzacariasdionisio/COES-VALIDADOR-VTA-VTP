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
    /// Clase de acceso a datos de la tabla SRM_RECOMENDACION
    /// </summary>
    public class SrmRecomendacionRepository: RepositoryBase, ISrmRecomendacionRepository
    {
        public SrmRecomendacionRepository(string strConn): base(strConn)
        {
        }

        SrmRecomendacionHelper helper = new SrmRecomendacionHelper();

        public int Save(SrmRecomendacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Srmreccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Srmcrtcodi, DbType.Int32, entity.Srmcrtcodi);
            dbProvider.AddInParameter(command, helper.Srmstdcodi, DbType.Int32, entity.Srmstdcodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Srmrecfecharecomend, DbType.DateTime, entity.Srmrecfecharecomend);
            dbProvider.AddInParameter(command, helper.Srmrecfechavencim, DbType.DateTime, entity.Srmrecfechavencim);
            dbProvider.AddInParameter(command, helper.Srmrecdianotifplazo, DbType.Int32, entity.Srmrecdianotifplazo);
            dbProvider.AddInParameter(command, helper.Srmrectitulo, DbType.String, entity.Srmrectitulo);
            dbProvider.AddInParameter(command, helper.Srmrecrecomendacion, DbType.String, entity.Srmrecrecomendacion);
            dbProvider.AddInParameter(command, helper.Srmrecactivo, DbType.String, entity.Srmrecactivo);
            dbProvider.AddInParameter(command, helper.Srmrecusucreacion, DbType.String, entity.Srmrecusucreacion);
            dbProvider.AddInParameter(command, helper.Srmrecfeccreacion, DbType.DateTime, entity.Srmrecfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmrecusumodificacion, DbType.String, entity.Srmrecusumodificacion);
            dbProvider.AddInParameter(command, helper.Srmrecfecmodificacion, DbType.DateTime, entity.Srmrecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Evenrcmctaf, DbType.String, entity.Evenrcmctaf);
            dbProvider.AddInParameter(command, helper.Afrrec, DbType.Int32, entity.Afrrec);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SrmRecomendacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Srmcrtcodi, DbType.Int32, entity.Srmcrtcodi);
            dbProvider.AddInParameter(command, helper.Srmstdcodi, DbType.Int32, entity.Srmstdcodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Srmrecfecharecomend, DbType.DateTime, entity.Srmrecfecharecomend);
            dbProvider.AddInParameter(command, helper.Srmrecfechavencim, DbType.DateTime, entity.Srmrecfechavencim);
            dbProvider.AddInParameter(command, helper.Srmrecdianotifplazo, DbType.Int32, entity.Srmrecdianotifplazo);
            dbProvider.AddInParameter(command, helper.Srmrectitulo, DbType.String, entity.Srmrectitulo);
            dbProvider.AddInParameter(command, helper.Srmrecrecomendacion, DbType.String, entity.Srmrecrecomendacion);
            dbProvider.AddInParameter(command, helper.Srmrecactivo, DbType.String, entity.Srmrecactivo);
            dbProvider.AddInParameter(command, helper.Srmrecusucreacion, DbType.String, entity.Srmrecusucreacion);
            dbProvider.AddInParameter(command, helper.Srmrecfeccreacion, DbType.DateTime, entity.Srmrecfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmrecusumodificacion, DbType.String, entity.Srmrecusumodificacion);
            dbProvider.AddInParameter(command, helper.Srmrecfecmodificacion, DbType.DateTime, entity.Srmrecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Srmreccodi, DbType.Int32, entity.Srmreccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int srmreccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Srmreccodi, DbType.Int32, srmreccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SrmRecomendacionDTO GetById(int srmreccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Srmreccodi, DbType.Int32, srmreccodi);
            SrmRecomendacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SrmRecomendacionDTO> List()
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
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

        public List<SrmRecomendacionDTO> GetByCriteria()
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
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
        /// Graba los datos de la tabla SRM_RECOMENDACION
        /// </summary>
        public int SaveSrmRecomendacionId(SrmRecomendacionDTO entity)
        {
            
                int id = 0;

                if (entity.Srmreccodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Srmreccodi;
                }

                return id;

            
        }

        public List<SrmRecomendacionDTO> BuscarOperaciones(int evenCodi,int equiCodi,int srmcrtcodi,int srmstdcodi,int usercode,DateTime srmrecFecharecomend,DateTime srmrecFechavencim, int nroPage, int pageSize)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListado, evenCodi,equiCodi,srmcrtcodi,srmstdcodi,usercode,srmrecFecharecomend.ToString(ConstantesBase.FormatoFecha),srmrecFechavencim.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iSrmreccodi = dr.GetOrdinal(this.helper.Srmreccodi);
                    if (!dr.IsDBNull(iSrmreccodi)) entity.Srmreccodi = Convert.ToInt32(dr.GetValue(iSrmreccodi));

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

                    int iUsercode = dr.GetOrdinal(this.helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

                    int iSrmrecfecharecomend = dr.GetOrdinal(this.helper.Srmrecfecharecomend);
                    if (!dr.IsDBNull(iSrmrecfecharecomend)) entity.Srmrecfecharecomend = dr.GetDateTime(iSrmrecfecharecomend);

                    int iSrmrecfechavencim = dr.GetOrdinal(this.helper.Srmrecfechavencim);
                    if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);

                    int iSrmrecdianotifplazo = dr.GetOrdinal(this.helper.Srmrecdianotifplazo);
                    if (!dr.IsDBNull(iSrmrecdianotifplazo)) entity.Srmrecdianotifplazo = Convert.ToInt32(dr.GetValue(iSrmrecdianotifplazo));

                    int iSrmrectitulo = dr.GetOrdinal(this.helper.Srmrectitulo);
                    if (!dr.IsDBNull(iSrmrectitulo)) entity.Srmrectitulo = dr.GetString(iSrmrectitulo);

                    int iSrmrecrecomendacion = dr.GetOrdinal(this.helper.Srmrecrecomendacion);
                    if (!dr.IsDBNull(iSrmrecrecomendacion)) entity.Srmrecrecomendacion = dr.GetString(iSrmrecrecomendacion);

                    int iSrmrecactivo = dr.GetOrdinal(this.helper.Srmrecactivo);
                    if (!dr.IsDBNull(iSrmrecactivo)) entity.Srmrecactivo = dr.GetString(iSrmrecactivo);

                    int iSrmrecusucreacion = dr.GetOrdinal(this.helper.Srmrecusucreacion);
                    if (!dr.IsDBNull(iSrmrecusucreacion)) entity.Srmrecusucreacion = dr.GetString(iSrmrecusucreacion);

                    int iSrmrecfeccreacion = dr.GetOrdinal(this.helper.Srmrecfeccreacion);
                    if (!dr.IsDBNull(iSrmrecfeccreacion)) entity.Srmrecfeccreacion = dr.GetDateTime(iSrmrecfeccreacion);

                    int iSrmrecusumodificacion = dr.GetOrdinal(this.helper.Srmrecusumodificacion);
                    if (!dr.IsDBNull(iSrmrecusumodificacion)) entity.Srmrecusumodificacion = dr.GetString(iSrmrecusumodificacion);

                    int iSrmrecfecmodificacion = dr.GetOrdinal(this.helper.Srmrecfecmodificacion);
                    if (!dr.IsDBNull(iSrmrecfecmodificacion)) entity.Srmrecfecmodificacion = dr.GetDateTime(iSrmrecfecmodificacion);

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iUsername = dr.GetOrdinal(this.helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int evenCodi,int equiCodi,int srmcrtcodi,int srmstdcodi,int usercode,DateTime srmrecFecharecomend,DateTime srmrecFechavencim)
        {
            String sql = String.Format(this.helper.TotalRegistros, evenCodi,equiCodi,srmcrtcodi,srmstdcodi,usercode,srmrecFecharecomend.ToString(ConstantesBase.FormatoFecha),srmrecFechavencim.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<SrmRecomendacionDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, string detRecomend, int estado,
            int criticidad, int nroPage, int pageSize)
        {

            int? eveniniOld = -1;

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoGestion, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));


                    //if (detRecomend == "N")
                    //{
                    //    if (entity.Srmstdcodi == 3)
                    //    {
                    //        continue;
                    //    }

                    //    if (eveniniOld == entity.Evencodi)
                    //    {
                    //        eveniniOld = entity.Evencodi;
                    //        continue;
                    //    }
                    //}
                    
                    eveniniOld = entity.Evencodi;

                    int iTipo = dr.GetOrdinal(this.helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    
                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(this.helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenAsunto = dr.GetOrdinal(this.helper.EvenAsunto);
                    if (!dr.IsDBNull(iEvenAsunto)) entity.EvenAsunto = dr.GetString(iEvenAsunto);

                    int iSrmrecfecharecomend = dr.GetOrdinal(this.helper.Srmrecfecharecomend);
                    if (!dr.IsDBNull(iSrmrecfecharecomend)) entity.Srmrecfecharecomend = dr.GetDateTime(iSrmrecfecharecomend);

                    int iSrmrecfechavencim = dr.GetOrdinal(this.helper.Srmrecfechavencim);
                    if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);



                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);


                    int iSrmrecdianotifplazo = dr.GetOrdinal(this.helper.Srmrecdianotifplazo);
                    if (!dr.IsDBNull(iSrmrecdianotifplazo)) entity.Srmrecdianotifplazo = Convert.ToInt32(dr.GetValue(iSrmrecdianotifplazo));

                    int iConComentario = dr.GetOrdinal(this.helper.ConComentario);
                    if (!dr.IsDBNull(iConComentario)) entity.ConComentario = dr.GetString(iConComentario);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, string detRecomend, int estado,
            int criticidad)
        {
            String sql = String.Format(this.helper.TotalRegistrosGestion , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<SrmRecomendacionDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int nroPage, int pageSize)
        {
                        

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoGestionFaltante, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iTipo = dr.GetOrdinal(this.helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(this.helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenAsunto = dr.GetOrdinal(this.helper.EvenAsunto);
                    if (!dr.IsDBNull(iEvenAsunto)) entity.EvenAsunto = dr.GetString(iEvenAsunto);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi)
        {
            String sql = String.Format(this.helper.TotalRegistrosGestionFaltante, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<SrmRecomendacionDTO> BuscarOperaciones(int evenCodi, string activo, int nroPage, int pageSize)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoRecomendacion, evenCodi, activo, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iSrmreccodi = dr.GetOrdinal(this.helper.Srmreccodi);
                    if (!dr.IsDBNull(iSrmreccodi)) entity.Srmreccodi = Convert.ToInt32(dr.GetValue(iSrmreccodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iSrmrectitulo = dr.GetOrdinal(this.helper.Srmrectitulo);
                    if (!dr.IsDBNull(iSrmrectitulo)) entity.Srmrectitulo = dr.GetString(iSrmrectitulo);

                    int iSrmrecfecharecomend = dr.GetOrdinal(this.helper.Srmrecfecharecomend);
                    if (!dr.IsDBNull(iSrmrecfecharecomend)) entity.Srmrecfecharecomend = dr.GetDateTime(iSrmrecfecharecomend);

                    int iSrmrecfechavencim = dr.GetOrdinal(this.helper.Srmrecfechavencim);
                    if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);
                                        
                    int iSrmrecrecomendacion = dr.GetOrdinal(this.helper.Srmrecrecomendacion);
                    if (!dr.IsDBNull(iSrmrecrecomendacion)) entity.Srmrecrecomendacion = dr.GetString(iSrmrecrecomendacion);
                    
                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);
                    
                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iComentario = dr.GetOrdinal(this.helper.Comentario);
                    if (!dr.IsDBNull(iComentario)) entity.Comentario = dr.GetString(iComentario);
                    
                   
                    int iSrmrecusucreacion = dr.GetOrdinal(this.helper.Srmrecusucreacion);
                    if (!dr.IsDBNull(iSrmrecusucreacion)) entity.Srmrecusucreacion = dr.GetString(iSrmrecusucreacion);

                    int iSrmrecfeccreacion = dr.GetOrdinal(this.helper.Srmrecfeccreacion);
                    if (!dr.IsDBNull(iSrmrecfeccreacion)) entity.Srmrecfeccreacion = dr.GetDateTime(iSrmrecfeccreacion);

                    int iSrmrecusumodificacion = dr.GetOrdinal(this.helper.Srmrecusumodificacion);
                    if (!dr.IsDBNull(iSrmrecusumodificacion)) entity.Srmrecusumodificacion = dr.GetString(iSrmrecusumodificacion);

                    int iSrmrecfecmodificacion = dr.GetOrdinal(this.helper.Srmrecfecmodificacion);
                    if (!dr.IsDBNull(iSrmrecfecmodificacion)) entity.Srmrecfecmodificacion = dr.GetDateTime(iSrmrecfecmodificacion);


                    int iSrmrecactivo = dr.GetOrdinal(this.helper.Srmrecactivo);
                    if (!dr.IsDBNull(iSrmrecactivo)) entity.Srmrecactivo = dr.GetString(iSrmrecactivo);

                    int iEvenrcmctaf = dr.GetOrdinal(this.helper.Evenrcmctaf);
                    if (!dr.IsDBNull(iEvenrcmctaf)) entity.Evenrcmctaf = dr.GetString(iEvenrcmctaf);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //reporte listado
        public List<SrmRecomendacionDTO> BuscarOperacionesReporteListado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
    int criticidad, string recomendacion, int usercode, int nroPage, int pageSize)
        {                      

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iSrmrectitulo = dr.GetOrdinal(this.helper.Srmrectitulo);
                    if (!dr.IsDBNull(iSrmrectitulo)) entity.Srmrectitulo = dr.GetString(iSrmrectitulo);

                    int iUsername = dr.GetOrdinal(this.helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iSrmrecfecharecomend = dr.GetOrdinal(this.helper.Srmrecfecharecomend);
                    if (!dr.IsDBNull(iSrmrecfecharecomend)) entity.Srmrecfecharecomend = dr.GetDateTime(iSrmrecfecharecomend);

                    int iSrmrecfechavencim = dr.GetOrdinal(this.helper.Srmrecfechavencim);
                    if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);

                    int iSrmrecdianotifplazo = dr.GetOrdinal(this.helper.Srmrecdianotifplazo);
                    if (!dr.IsDBNull(iSrmrecdianotifplazo)) entity.Srmrecdianotifplazo = Convert.ToInt32(dr.GetValue(iSrmrecdianotifplazo));

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    
                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                          
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
    int criticidad, string recomendacion, int usercode)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.TotalRegistrosListadoReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
                        
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<SrmRecomendacionDTO> BuscarOperacionesEmpresaCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoEmpresaCriticidad, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi)); 

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SrmRecomendacionDTO> BuscarOperacionesEmpresaEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
    int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoEmpresaEstado, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();


                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SrmRecomendacionDTO> BuscarOperacionesTipoEquipoCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
    int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoTipoEquipoCriticidad, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SrmRecomendacionDTO> BuscarOperacionesTipoEquipoEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoTipoEquipoEstado, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();
                                        

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);


                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<SrmRecomendacionDTO> BuscarOperacionesEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoEstado, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);


                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SrmRecomendacionDTO> BuscarOperacionesEstadoCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
    int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoEstadoCriticidad, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();


                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SrmRecomendacionDTO> BuscarOperacionesCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
int criticidad, string recomendacion, int usercode)
        {

            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoCriticidad, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, estado, criticidad, recomendacion, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iRegistros = dr.GetOrdinal(this.helper.Registros);
                    if (!dr.IsDBNull(iRegistros)) entity.Registros = Convert.ToInt32(dr.GetValue(iRegistros));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public int ObtenerNroFilas(int evenCodi, string activo)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.TotalRegistrosRec, evenCodi, activo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
                        
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        public List<SrmRecomendacionDTO> BuscarOperacionesAlarma(DateTime fecha, int reporteDiaVencimiento, int repeticionAlarma)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoAlarma, fecha.ToString(ConstantesBase.FormatoFecha), reporteDiaVencimiento, repeticionAlarma);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iSrmreccodi = dr.GetOrdinal(this.helper.Srmreccodi);
                    if (!dr.IsDBNull(iSrmreccodi)) entity.Srmreccodi = Convert.ToInt32(dr.GetValue(iSrmreccodi));

                    int iAvencer = dr.GetOrdinal(this.helper.Avencer);
                    if (!dr.IsDBNull(iAvencer)) entity.Avencer = Convert.ToInt32(dr.GetValue(iAvencer));
                    int iVencido = dr.GetOrdinal(this.helper.Vencido);
                    if (!dr.IsDBNull(iVencido)) entity.Vencido = Convert.ToInt32(dr.GetValue(iVencido));
                    int iCiclico = dr.GetOrdinal(this.helper.Ciclico);
                    if (!dr.IsDBNull(iCiclico)) entity.Ciclico = Convert.ToInt32(dr.GetValue(iCiclico));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iSrmrectitulo = dr.GetOrdinal(this.helper.Srmrectitulo);
                    if (!dr.IsDBNull(iSrmrectitulo)) entity.Srmrectitulo = dr.GetString(iSrmrectitulo);

                    int iSrmrecrecomendacion = dr.GetOrdinal(this.helper.Srmrecrecomendacion);
                    if (!dr.IsDBNull(iSrmrecrecomendacion)) entity.Srmrecrecomendacion = dr.GetString(iSrmrecrecomendacion);


                    int iSrmrecfechavencim = dr.GetOrdinal(this.helper.Srmrecfechavencim);
                    if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);


                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);
                                        

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRecomendacionCtaf(int Afrrec)
        {
            String sql = String.Format(this.helper.TotalRegistrosCtaf, Afrrec);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
        public List<SrmRecomendacionDTO> BuscarOperacionesCtaf(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int nroPage, int pageSize)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ObtenerListadoGestionCtaf, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , famcodi, equiAbrev, tipoEmpresa, emprcodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

                    int iTipo = dr.GetOrdinal(this.helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(this.helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenAsunto = dr.GetOrdinal(this.helper.EvenAsunto);
                    if (!dr.IsDBNull(iEvenAsunto)) entity.EvenAsunto = dr.GetString(iEvenAsunto);

                    int iSrmrecfecharecomend = dr.GetOrdinal(this.helper.Srmrecfecharecomend);
                    if (!dr.IsDBNull(iSrmrecfecharecomend)) entity.Srmrecfecharecomend = dr.GetDateTime(iSrmrecfecharecomend);

                    int iSrmrecfechavencim = dr.GetOrdinal(this.helper.Srmrecfechavencim);
                    if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iSrmrecdianotifplazo = dr.GetOrdinal(this.helper.Srmrecdianotifplazo);
                    if (!dr.IsDBNull(iSrmrecdianotifplazo)) entity.Srmrecdianotifplazo = Convert.ToInt32(dr.GetValue(iSrmrecdianotifplazo));

                    int iConComentario = dr.GetOrdinal(this.helper.ConComentario);
                    if (!dr.IsDBNull(iConComentario)) entity.ConComentario = dr.GetString(iConComentario);

                    int iEvenrcmctaf = dr.GetOrdinal(this.helper.Evenrcmctaf);
                    if (!dr.IsDBNull(iEvenrcmctaf)) entity.Evenrcmctaf = dr.GetString(iEvenrcmctaf);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<SrmRecomendacionDTO> ListadoRecomendacionesEventosCtaf(int evencodi)
        {
            List<SrmRecomendacionDTO> entitys = new List<SrmRecomendacionDTO>();
            String sql = String.Format(this.helper.ListadoRecomendacionesEventosCtaf, evencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iSrmreccodi = dr.GetOrdinal(this.helper.Srmreccodi);
                    if (!dr.IsDBNull(iSrmreccodi)) entity.Srmreccodi = Convert.ToInt32(dr.GetValue(iSrmreccodi));

                    int iEvenrcmctaf = dr.GetOrdinal(this.helper.Evenrcmctaf);
                    if (!dr.IsDBNull(iEvenrcmctaf)) entity.Evenrcmctaf = dr.GetString(iEvenrcmctaf);

                    int iAfrrec = dr.GetOrdinal(this.helper.Afrrec);
                    if (!dr.IsDBNull(iAfrrec)) entity.Afrrec = Convert.ToInt32(dr.GetValue(iAfrrec));

                    int iSrmrecactivo = dr.GetOrdinal(this.helper.Srmrecactivo);
                    if (!dr.IsDBNull(iSrmrecactivo)) entity.Srmrecactivo = dr.GetString(iSrmrecactivo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public SrmRecomendacionDTO GetByIdxAfrrec(int afrrec)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByIdxAfrrec);

            dbProvider.AddInParameter(command, helper.Afrrec, DbType.Int32, afrrec);
            SrmRecomendacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEvenrcmctaf = dr.GetOrdinal(helper.Evenrcmctaf);
                    if (!dr.IsDBNull(iEvenrcmctaf)) entity.Evenrcmctaf = dr.GetString(iEvenrcmctaf);
                }
            }

            return entity;
        }
        public int ValidaRecomendacionxEventoEstado(int evenCodi, int srmstdcodi)
        {
            String sql = String.Format(helper.ValidaRecomendacionxEventoEstado, evenCodi, srmstdcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
        public int ObtenerRecomendacionEvento(int Evencodi, int Equicodi, int Srmcrtcodi, int Srmstdcodi)
        {
            String sql = String.Format(this.helper.ObtenerRecomendacionEvento, Evencodi, Equicodi, Srmcrtcodi, Srmstdcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}

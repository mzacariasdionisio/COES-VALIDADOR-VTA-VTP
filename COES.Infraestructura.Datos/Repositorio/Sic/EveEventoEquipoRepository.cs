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
    public class EveEventoEquipoRepository : RepositoryBase, IEveEventoEquipoRepository
    {
        public EveEventoEquipoRepository(string strConn): base(strConn)
        {
        }

        EveEventoEquipoHelper helper = new EveEventoEquipoHelper();

        public int Save(EveEventoEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eeqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Eeqfechaini, DbType.DateTime, entity.Eeqfechaini);
            dbProvider.AddInParameter(command, helper.Eeqestado, DbType.Int32, entity.Eeqestado);
            dbProvider.AddInParameter(command, helper.Eeqdescripcion, DbType.String, entity.Eeqdescripcion);
            dbProvider.AddInParameter(command, helper.Eeqfechafin, DbType.DateTime, entity.Eeqfechafin);
            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }


        public int Aprobar(EveEventoEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlAprobarEquiposSEIN);
    
            dbProvider.AddInParameter(command, helper.Eeqcodi, DbType.Int32, entity.Eeqcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Subcausadesc, DbType.String, entity.Subcausadesc);
            dbProvider.AddInParameter(command, helper.Eeqfechaini, DbType.DateTime, entity.Eeqfechaini);
            dbProvider.AddInParameter(command, helper.Eeqestado, DbType.Int32, entity.Eeqestado);
            dbProvider.AddInParameter(command, helper.Eeqdescripcion, DbType.String, entity.Eeqdescripcion);
            dbProvider.AddInParameter(command, helper.Eeqfechafin, DbType.DateTime, entity.Eeqfechafin);


            dbProvider.ExecuteNonQuery(command);
            return entity.Eeqcodi;
        }


        public List<EveEventoEquipoDTO> ListarDetalleEquiposSEIN(string empresas, int nroPaginas,
                int pageSize, string idsFamilia, string campo, string orden)
        {
            List<EveEventoEquipoDTO> entitys = new List<EveEventoEquipoDTO>();
            string query = string.Format(helper.SqlListarDetalleEquiposSEIN, empresas,
                nroPaginas, pageSize, idsFamilia, campo, orden);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveEventoEquipoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveEventoEquipoDTO();
                    entity = helper.Create(dr);
                    
                    int iEeqcodi = dr.GetOrdinal(helper.Eeqcodi);
                    if (!dr.IsDBNull(iEeqcodi)) entity.Eeqcodi = Convert.ToInt32(dr.GetValue(iEeqcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iEeqfechaini = dr.GetOrdinal(helper.Eeqfechaini);
                    if (!dr.IsDBNull(iEeqfechaini)) entity.Eeqfechaini = dr.GetDateTime(iEeqfechaini);

                    int iEeqestado = dr.GetOrdinal(helper.Eeqestado);
                    if (!dr.IsDBNull(iEeqestado)) entity.Eeqestado = Convert.ToInt32(dr.GetValue(iEeqestado));

                    int iEeqdescripcion = dr.GetOrdinal(helper.Eeqdescripcion);
                    if (!dr.IsDBNull(iEeqdescripcion)) entity.Eeqdescripcion = dr.GetString(iEeqdescripcion);

                    int iEeqfechafin = dr.GetOrdinal(helper.Eeqfechafin);
                    if (!dr.IsDBNull(iEeqfechafin)) entity.Eeqfechafin = dr.GetDateTime(iEeqfechafin);




                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEquicodi)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);




                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveEventoEquipoDTO> ListarPendientesEquiposSEIN(string empresas, int nroPaginas,
                int pageSize, string idsFamilia, string fechaini, string fechafin, string orden)
        {
            List<EveEventoEquipoDTO> entitys = new List<EveEventoEquipoDTO>();
            string query = string.Format(helper.SqlListarPendientesEquiposSEIN, empresas,
                nroPaginas, pageSize, idsFamilia, fechaini, fechafin, orden);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveEventoEquipoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveEventoEquipoDTO();
                    entity = helper.Create(dr);

                    int iEeqcodi = dr.GetOrdinal(helper.Eeqcodi);
                    if (!dr.IsDBNull(iEeqcodi)) entity.Eeqcodi = Convert.ToInt32(dr.GetValue(iEeqcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iEeqfechaini = dr.GetOrdinal(helper.Eeqfechaini);
                    if (!dr.IsDBNull(iEeqfechaini)) entity.Eeqfechaini = dr.GetDateTime(iEeqfechaini);

                    int iEeqestado = dr.GetOrdinal(helper.Eeqestado);
                    if (!dr.IsDBNull(iEeqestado)) entity.Eeqestado = Convert.ToInt32(dr.GetValue(iEeqestado));

                    int iEeqdescripcion = dr.GetOrdinal(helper.Eeqdescripcion);
                    if (!dr.IsDBNull(iEeqdescripcion)) entity.Eeqdescripcion = dr.GetString(iEeqdescripcion);

                    int iEeqfechafin = dr.GetOrdinal(helper.Eeqfechafin);
                    if (!dr.IsDBNull(iEeqfechafin)) entity.Eeqfechafin = dr.GetDateTime(iEeqfechafin);




                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEquicodi)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);




                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int AprobarE(int codigo, int idempresa,
            int idFamilia, int idequipo, int estado, int idmotivo, string motivoabrev, string ifecha, int idubicacion, string usuario)
        {
            int id = 1;
            int codProp = 0;
           

            string query = string.Format(helper.SqlUpdEstadoEquiposSEIN, codigo, idempresa,
             idFamilia, idequipo, estado, idmotivo, motivoabrev, ifecha, idubicacion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteScalar(command);


            string queryP = string.Format(helper.SqlGetPropEquiposSEIN, codigo, idempresa,
             idFamilia, idequipo, estado, idmotivo, motivoabrev, ifecha, idubicacion);

             command = dbProvider.GetSqlStringCommand(queryP);

             object result = dbProvider.ExecuteScalar(command);
             if (result != null) codProp = Convert.ToInt32(result);

             string fechaActual = Convert.ToString(DateTime.Now);
             if (fechaActual.Contains(". m.")) 
             {
                 fechaActual = fechaActual.Replace(". m.", ".m.");
             }

             string queryAct = string.Format(helper.SqlInsPropEqEquiposSEIN, codProp, idempresa,
              idFamilia, idequipo, estado, motivoabrev, fechaActual, ifecha, idubicacion);

             command = dbProvider.GetSqlStringCommand(queryAct);


             dbProvider.ExecuteScalar(command);


             if (motivoabrev == "EQREUBIC") 
             {
                 string fechaUp = Convert.ToString(DateTime.Now.AddSeconds(1));
                 if (fechaUp.Contains(". m."))
                 {
                     fechaUp = fechaUp.Replace(". m.", ".m.");
                 }
                 
                 string queryUbi = string.Format(helper.SqlInsPropEqEquiposSEIN, codProp, idempresa,
              idFamilia, idequipo, estado, motivoabrev, fechaUp, idubicacion, ifecha);

                 command = dbProvider.GetSqlStringCommand(queryUbi);


                 dbProvider.ExecuteScalar(command);
                 

                 string queryEq = string.Format(helper.SqlUpdUbicEqEquiposSEIN, codProp, idempresa,
              idFamilia, idequipo, estado, motivoabrev, usuario, idubicacion, fechaUp);

                 command = dbProvider.GetSqlStringCommand(queryEq);


                 dbProvider.ExecuteScalar(command);



             }

            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public List<EveEventoEquipoDTO> ListarDetalleEquiposSEIN02(string equipos, string sTipoEquipo, DateTime fechaini, DateTime fechafin)
        {
            List<EveEventoEquipoDTO> entitys = new List<EveEventoEquipoDTO>();
            string query = string.Format(helper.SqlListarDetalleEquiposSEIN02, equipos, sTipoEquipo,
                 fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveEventoEquipoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveEventoEquipoDTO();
                    entity = helper.Create(dr);

                    int iEeqcodi = dr.GetOrdinal(helper.Eeqcodi);
                    if (!dr.IsDBNull(iEeqcodi)) entity.Eeqcodi = Convert.ToInt32(dr.GetValue(iEeqcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iEeqfechaini = dr.GetOrdinal(helper.Eeqfechaini);
                    if (!dr.IsDBNull(iEeqfechaini)) entity.Eeqfechaini = dr.GetDateTime(iEeqfechaini);

                    int iEeqestado = dr.GetOrdinal(helper.Eeqestado);
                    if (!dr.IsDBNull(iEeqestado)) entity.Eeqestado = Convert.ToInt32(dr.GetValue(iEeqestado));

                    int iEeqdescripcion = dr.GetOrdinal(helper.Eeqdescripcion);
                    if (!dr.IsDBNull(iEeqdescripcion)) entity.Eeqdescripcion = dr.GetString(iEeqdescripcion);

                    int iEeqfechafin = dr.GetOrdinal(helper.Eeqfechafin);
                    if (!dr.IsDBNull(iEeqfechafin)) entity.Eeqfechafin = dr.GetDateTime(iEeqfechafin);


                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEquicodi)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreanomb = dr.GetOrdinal("AREANOMB");
                    if (!dr.IsDBNull(iFamnomb)) entity.Areanomb = dr.GetString(iAreanomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveEventoEquipoDTO> ListarIngresoSalidaOperacionComercialSEIN(int subcausacodi, DateTime fechaini, DateTime fechafin)
        {
            List<EveEventoEquipoDTO> entitys = new List<EveEventoEquipoDTO>();
            string query = string.Format(helper.SqlListarIngresoSalidaOperacionComercialSEIN,
                 fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha),  subcausacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveEventoEquipoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveEventoEquipoDTO();
                    //entity = helper.Create(dr);

                    int iTgenernomb = dr.GetOrdinal("TGENERNOMB");
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iCtgcodi = dr.GetOrdinal("CTGCODI");
                    if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));

                    int iFenergcodi = dr.GetOrdinal("FENERGCODI");
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal("FENERGNOMB");
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iCtgdetnomb = dr.GetOrdinal("CTGDETNOMB");
                    if (!dr.IsDBNull(iCtgdetnomb)) entity.Ctgdetnomb = dr.GetString(iCtgdetnomb);


                    int iEeqcodi = dr.GetOrdinal(helper.Eeqcodi);
                    if (!dr.IsDBNull(iEeqcodi)) entity.Eeqcodi = Convert.ToInt32(dr.GetValue(iEeqcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    
                    int iEeqfechaini = dr.GetOrdinal(helper.Eeqfechaini);
                    if (!dr.IsDBNull(iEeqfechaini)) entity.Eeqfechaini = dr.GetDateTime(iEeqfechaini);

                    int iEeqfechafin = dr.GetOrdinal(helper.Eeqfechafin);
                    if (!dr.IsDBNull(iEeqfechafin)) entity.Eeqfechafin = dr.GetDateTime(iEeqfechafin);
                   
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEquicodi)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquinombpadre = dr.GetOrdinal("EQUINOMBPADRE");
                    if (!dr.IsDBNull(iEquinombpadre)) entity.Equinombpadre = dr.GetString(iEquinombpadre);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEquitension = dr.GetOrdinal("EQUITENSION");
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = Convert.ToDecimal(dr.GetValue(iEquitension));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
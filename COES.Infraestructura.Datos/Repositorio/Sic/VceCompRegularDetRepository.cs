using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class VceCompRegularDetRepository : RepositoryBase, IVceCompRegularDetRepository
    {
        public VceCompRegularDetRepository(string strConn) : base(strConn)
        {
        }

        VceCompRegularDetHelper helper = new VceCompRegularDetHelper();

        public void Save(VceCompRegularDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crdettipocalc, DbType.String, entity.Crdettipocalc);
            dbProvider.AddInParameter(command, helper.Crdetcvtbajaefic, DbType.Decimal, entity.Crdetcvtbajaefic);
            dbProvider.AddInParameter(command, helper.Crdetcompensacion, DbType.Decimal, entity.Crdetcompensacion);
            dbProvider.AddInParameter(command, helper.Crdetcmg, DbType.Decimal, entity.Crdetcmg);
            dbProvider.AddInParameter(command, helper.Crdetcvt, DbType.Decimal, entity.Crdetcvt);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Crdetvalor, DbType.Decimal, entity.Crdetvalor);
            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, entity.Crdethora);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceCompRegularDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crdettipocalc, DbType.String, entity.Crdettipocalc);
            dbProvider.AddInParameter(command, helper.Crdetcvtbajaefic, DbType.Decimal, entity.Crdetcvtbajaefic);
            dbProvider.AddInParameter(command, helper.Crdetcompensacion, DbType.Decimal, entity.Crdetcompensacion);
            dbProvider.AddInParameter(command, helper.Crdetcmg, DbType.Decimal, entity.Crdetcmg);
            dbProvider.AddInParameter(command, helper.Crdetcvt, DbType.Decimal, entity.Crdetcvt);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Crdetvalor, DbType.Decimal, entity.Crdetvalor);
            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, entity.Crdethora);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime crdethora, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, crdethora);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }
       
        public void DeleteByGroup(int pecacodi, int grupocodi, int subcausacodi, DateTime crcbehorini, DateTime crcbehorfin)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByGroup);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, subcausacodi);
            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, crcbehorini);
            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, crcbehorfin);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceCompRegularDetDTO GetById(DateTime crdethora, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, crdethora);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            VceCompRegularDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceCompRegularDetDTO> List()
        {
            List<VceCompRegularDetDTO> entitys = new List<VceCompRegularDetDTO>();
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

        public List<VceCompRegularDetDTO> GetByCriteria()
        {
            List<VceCompRegularDetDTO> entitys = new List<VceCompRegularDetDTO>();
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

        public List<VceCompRegularDetDTO> ListCompensacionesEspeciales(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            string condicion = "";

            if (!empresa.Equals("") && empresa != null)
            {
                condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
            }

            if (!central.Equals("") && central != null)
            {
                condicion = condicion + " AND CG.EQUICODI = " + central;
            }

            if (!grupo.Equals("") && grupo != null)
            {
                condicion = condicion + " AND GG.GRUPOCODI = " + grupo;
            }

            if (!modo.Equals("") && modo != null)
            {
                condicion = condicion + " AND MO.GRUPOCODI = " + modo;
            }
      
            //--Periodo
            //AND AND pecacodi = 18 --<pecacodi>
            //--Empresa
            //AND EMP.EMPRCODI = 10 --<EMPRCODI>
            //--Central
            //AND MO.GRUPOPADRE IN (SELECT GRUPOCODI FROM PR_GRUPO WHERE GRUPOPADRE = 28)-- <ID_CENTRAL>
            //--Grupo
            //AND MO.GRUPOPADRE = 138 -- <ID_GRUPO>
            //--Modo Operación
            //AND MO.GRUPOCODI = 209 -- <ID_MODO OPERACION>
            //--Tipo Operación / Calificación


            List<VceCompRegularDetDTO> entitys = new List<VceCompRegularDetDTO>();
            string queryString = string.Format(helper.SqlListCompensacionesEspeciales, pecacodi, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompRegularDetDTO entity = new VceCompRegularDetDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iVceCrdCompensacion = dr.GetOrdinal(helper.Crdetcompensacion);
                    if (!dr.IsDBNull(iVceCrdCompensacion)) entity.Crdetcompensacion = dr.GetDecimal(iVceCrdCompensacion);

                    int iGrupoCodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void ProcesarCompensacionEspecial(int pecacodi)
        {
            //Obtenemos la lista de empresas
            string queryString = string.Format(helper.SqlListFechaMod, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            List<DateTime> listFechas = new List<DateTime>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    listFechas.Add(dr.GetDateTime(dr.GetOrdinal("CRCVFECMOD")));
                }
            }

            //Actualizamos los costos variables
            foreach (DateTime fecha in listFechas)
            {
                string sql = "UPDATE VCE_COMP_REGULAR_DET SET (CRDETCVT) = (SELECT CRCVCVT FROM VCE_COSTO_VARIABLE CV ";
                sql = sql + " WHERE CV.pecacodi = VCE_COMP_REGULAR_DET.pecacodi AND CV.GRUPOCODI = VCE_COMP_REGULAR_DET.GRUPOCODI ";
                sql = sql + " AND CV.CRCVFECMOD = TO_DATE('" + fecha.ToString("dd/MM/yyyy") + "', 'DD/MM/YYYY'))";
                sql = sql + " WHERE pecacodi = " + pecacodi;
                sql = sql + " AND GRUPOCODI IN (SELECT GRUPOCODI FROM VCE_COSTO_VARIABLE CV ";
                sql = sql + " WHERE CV.pecacodi = " + pecacodi + " AND CV.CRCVFECMOD = TO_DATE('" + fecha.ToString("dd/MM/yyyy") + "', 'DD/MM/YYYY'))";
                sql = sql + " AND CRDETHORA >= TO_DATE('" + fecha.ToString("dd/MM/yyyy") + "', 'DD/MM/YYYY')";

                DbCommand commandUpdateCV = dbProvider.GetSqlStringCommand(sql);
                dbProvider.ExecuteNonQuery(commandUpdateCV);
            }

            //Actualizamos los costos marginales
            for (int i = 1; i <= 95; i++)
            {
                string sql = "UPDATE VCE_COMP_REGULAR_DET SET CRDETCMG = (SELECT COSMAR" + i + " FROM TRN_COSTO_MARGINAL CM JOIN VCE_PERIODO_CALCULO PC ON CM.PERICODI = PC.PERICODI JOIN VCE_DATCALCULO DCM ON PC.PECACODI = DCM.PECACODI AND CM.BARRCODI = DCM.BARRCODI AND TO_CHAR(DCM.CRDCGFECMOD,'DD')='01'";
                sql = sql + " WHERE VCE_COMP_REGULAR_DET.pecacodi = PC.pecacodi AND VCE_COMP_REGULAR_DET.GRUPOCODI = DCM.GRUPOCODI ";
                sql = sql + " AND CM.COSMARVERSION = PC.PECAVERSIONVTEA";
                sql = sql + " AND TO_CHAR(VCE_COMP_REGULAR_DET.CRDETHORA,'DD') = LPAD(CM.COSMARDIA,2,'0'))";
                sql = sql + " WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI IS NOT NULL AND CRDETVALOR IS NOT NULL";
                sql = sql + " AND TO_CHAR(CRDETHORA,'HH24:MI') = TO_CHAR(TRUNC(SYSDATE)+(1/24/60)*15*" + i + ",'HH24:MI')";
                
                DbCommand commandUpdateCM = dbProvider.GetSqlStringCommand(sql);
                dbProvider.ExecuteNonQuery(commandUpdateCM);
            }

            //Actualizamos la medición 96
            string query = " UPDATE VCE_COMP_REGULAR_DET SET CRDETCMG = (SELECT COSMAR96 FROM TRN_COSTO_MARGINAL CM JOIN VCE_PERIODO_CALCULO PC ON CM.PERICODI = PC.PERICODI JOIN VCE_DATCALCULO DCM ON PC.PECACODI = DCM.PECACODI AND CM.BARRCODI = DCM.BARRCODI AND TO_CHAR(DCM.CRDCGFECMOD,'DD')='01'";
            query = query + " WHERE VCE_COMP_REGULAR_DET.pecacodi = PC.pecacodi AND VCE_COMP_REGULAR_DET.GRUPOCODI = DCM.GRUPOCODI ";
            query = query + " AND CM.COSMARVERSION = PC.PECAVERSIONVTEA";
            query = query + " AND TO_CHAR(VCE_COMP_REGULAR_DET.CRDETHORA-1,'DD') = LPAD(CM.COSMARDIA,2,'0'))";
            query = query + " WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI IS NOT NULL AND CRDETVALOR IS NOT NULL";
            query = query + " AND TO_CHAR(CRDETHORA-1,'HH24:MI') = TO_CHAR(TRUNC(SYSDATE)+(1/24/60)*15*96,'HH24:MI')";

            DbCommand commandUpdateME96 = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandUpdateME96);


            //Calculamos la compensación POR BAJA EFICIENCIA
            query = "UPDATE VCE_COMP_REGULAR_DET SET CRDETCOMPENSACION = (CRDETCVT-CRDETCMG)*CRDETVALOR*1000";
            query = query + " WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI = 101";
            query = query + " AND CRDETCVT >= CRDETCMG";

            DbCommand commandCalculo = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandCalculo);
        }

        //MZD Start Compensacion MME
        #region Compensaciones MME
        public List<VceCompRegularDetDTO> ListCompensacionesMME(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            string condicion = "";

            if (!empresa.Equals("") && empresa != null)
            {
                condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
            }

            if (!central.Equals("") && central != null)
            {
                condicion = condicion + " AND CG.EQUICODI = " + central;
            }

            if (!grupo.Equals("") && grupo != null)
            {
                condicion = condicion + " AND GG.GRUPOCODI = " + grupo;
            }

            if (!modo.Equals("") && modo != null)
            {
                condicion = condicion + " AND MO.GRUPOCODI = " + modo;
            }

            //--Periodo
            //AND AND pecacodi = 18 --<pecacodi>
            //--Empresa
            //AND EMP.EMPRCODI = 10 --<EMPRCODI>
            //--Central
            //AND MO.GRUPOPADRE IN (SELECT GRUPOCODI FROM PR_GRUPO WHERE GRUPOPADRE = 28)-- <ID_CENTRAL>
            //--Grupo
            //AND MO.GRUPOPADRE = 138 -- <ID_GRUPO>
            //--Modo Operación
            //AND MO.GRUPOCODI = 209 -- <ID_MODO OPERACION>
            //--Tipo Operación / Calificación


            List<VceCompRegularDetDTO> entitys = new List<VceCompRegularDetDTO>();
            string queryString = string.Format(helper.SqlListCompensacionesEspeciales, pecacodi, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompRegularDetDTO entity = new VceCompRegularDetDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iVceCrdCompensacion = dr.GetOrdinal(helper.Crdetcompensacion);
                    if (!dr.IsDBNull(iVceCrdCompensacion)) entity.Crdetcompensacion = dr.GetDecimal(iVceCrdCompensacion);

                    int iGrupoCodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public void ProcesarCompensacionMME(int pecacodi)
        {
            //Actualizamos las potencias
            string queryString = string.Format(helper.SqlActualizarPotencia, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            //Obtenemos los diferentes modos de operacion
            string queryStringGrupoCodi = string.Format(helper.SqlListGrupoCodi, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryStringGrupoCodi);

            int grupocodi = 0;
            using (IDataReader drGrupoCodi = dbProvider.ExecuteReader(command))
            {
                while (drGrupoCodi.Read())
                {
                    grupocodi = drGrupoCodi.GetInt32(drGrupoCodi.GetOrdinal("GRUPOCODI"));


                    //Obtenemos la lista de fechas de modificación
                    queryString = string.Format(helper.SqlListFechaMod, pecacodi, grupocodi);
                    command = dbProvider.GetSqlStringCommand(queryString);

                    List<DateTime> listFechas = new List<DateTime>();
                    using (IDataReader dr = dbProvider.ExecuteReader(command))
                    {
                        while (dr.Read())
                        {
                            listFechas.Add(dr.GetDateTime(dr.GetOrdinal("CRCVFECMOD")));
                        }
                    }

                    var listaFechasArray = listFechas.ToArray();
                    string fechaIni = "";
                    string fechaFin = "";
                    for (int i=0;i<listFechas.Count;i++)
                    {
                        fechaIni = listFechas[i].ToString("dd/MM/yyyy");
                        fechaFin = "";
                        if (i< listFechas.Count-1)
                        {
                            fechaFin = string.Format(" AND ( CRDETHORA  - 1/86400 < TO_DATE('{0}', 'DD/MM/YYYY')) ", listFechas[i + 1].ToString("dd/MM/yyyy"));
                        }
                            

                        queryString = string.Format(helper.SqlActualizarPotenciaMinima, pecacodi, grupocodi, fechaIni, fechaFin);
                        command = dbProvider.GetSqlStringCommand(queryString);
                        dbProvider.ExecuteNonQuery(command);

                        queryString = string.Format(helper.SqlActualizarPotenciaMaxima, pecacodi, grupocodi, fechaIni, fechaFin);
                        command = dbProvider.GetSqlStringCommand(queryString);
                        dbProvider.ExecuteNonQuery(command);

                    }

                    //Actualizamos los consumos
                    foreach (DateTime fecha in listFechas)
                    {
                        //En base a la potencia medida
                        queryString = string.Format(helper.SqlActualizarConsumoCombustible, pecacodi, fecha.ToString("dd/MM/yyyy"), grupocodi);
                        command = dbProvider.GetSqlStringCommand(queryString);
                        dbProvider.ExecuteNonQuery(command);

                        //En base a la potencia efectiva
                        queryString = string.Format(helper.SqlActualizarPotenciaEfectiva, pecacodi, fecha.ToString("dd/MM/yyyy"), grupocodi);
                        command = dbProvider.GetSqlStringCommand(queryString);
                        dbProvider.ExecuteNonQuery(command);
                    }


                    //Actualizamos los datos técnicos del generador
                    foreach (DateTime fecha in listFechas)
                    {
                        queryString = string.Format(helper.SqlActualizarParametrosGenerador, pecacodi, fecha.ToString("dd/MM/yyyy"), grupocodi);
                        command = dbProvider.GetSqlStringCommand(queryString);
                        dbProvider.ExecuteNonQuery(command);
                    }

                    


                }
            }

            //Actualizamos el costo variable combustible
            queryString = string.Format(helper.SqlActualizarCVC, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el costo variable total
            queryString = string.Format(helper.SqlActualizarCVT, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);


            //Actualizamos los costos marginales
            for (int i = 1; i <= 95; i++)
            {
                string sql = "UPDATE VCE_COMP_REGULAR_DET SET CRDETCMG = (SELECT COSMAR" + i + " FROM TRN_COSTO_MARGINAL CM JOIN VCE_PERIODO_CALCULO PC ON CM.PERICODI = PC.PERICODI JOIN VCE_DATCALCULO DCM ON PC.PECACODI = DCM.PECACODI AND CM.BARRCODI = DCM.BARRCODI AND TO_CHAR(DCM.CRDCGFECMOD,'DD')='01'";
                sql = sql + " WHERE VCE_COMP_REGULAR_DET.pecacodi = PC.pecacodi AND VCE_COMP_REGULAR_DET.GRUPOCODI = DCM.GRUPOCODI ";
                sql = sql + " AND CM.COSMARVERSION = PC.PECAVERSIONVTEA";
                sql = sql + " AND TO_CHAR(VCE_COMP_REGULAR_DET.CRDETHORA,'DD') = LPAD(CM.COSMARDIA,2,'0'))";
                sql = sql + " WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI IS NOT NULL AND CRDETVALOR IS NOT NULL";
                sql = sql + " AND TO_CHAR(CRDETHORA,'HH24:MI') = TO_CHAR(TRUNC(SYSDATE)+(1/24/60)*15*" + i + ",'HH24:MI')";

                DbCommand commandUpdateCM = dbProvider.GetSqlStringCommand(sql);
                dbProvider.ExecuteNonQuery(commandUpdateCM);
            }

            //Actualizamos la medición 96
            string query = " UPDATE VCE_COMP_REGULAR_DET SET CRDETCMG = (SELECT COSMAR96 FROM TRN_COSTO_MARGINAL CM JOIN VCE_PERIODO_CALCULO PC ON CM.PERICODI = PC.PERICODI JOIN VCE_DATCALCULO DCM ON PC.PECACODI = DCM.PECACODI AND CM.BARRCODI = DCM.BARRCODI AND TO_CHAR(DCM.CRDCGFECMOD,'DD')='01'";
            query = query + " WHERE VCE_COMP_REGULAR_DET.pecacodi = PC.pecacodi AND VCE_COMP_REGULAR_DET.GRUPOCODI = DCM.GRUPOCODI ";
            query = query + " AND CM.COSMARVERSION = PC.PECAVERSIONVTEA";
            query = query + " AND TO_CHAR(VCE_COMP_REGULAR_DET.CRDETHORA-1,'DD') = LPAD(CM.COSMARDIA,2,'0'))";
            query = query + " WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI IS NOT NULL AND CRDETVALOR IS NOT NULL";
            query = query + " AND TO_CHAR(CRDETHORA-1,'HH24:MI') = TO_CHAR(TRUNC(SYSDATE)+(1/24/60)*15*96,'HH24:MI')";

            DbCommand commandUpdateME96 = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandUpdateME96);


            //Calculamos la compensación 
            query = "UPDATE VCE_COMP_REGULAR_DET SET CRDETCOMPENSACION = (CRDETCVT-CRDETCMG)*CRDETVALOR*1000";
            query = query + " WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI NOT IN (106,342,122)";//No considerar "POR PRUEBAS" ni "POR COGENERACION" ni "POR RESTRICCION OPERATIVA TEMPORAL"
            query = query + " AND CRDETCVT >= CRDETCMG";

            DbCommand commandCalculo = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandCalculo);




        }
        #endregion
        //MZD End Compensacion MME

        public void DeleteCompensacionManual(int pecacodi, int grupocodi, DateTime crdethora)
        {
            string queryString = string.Format(helper.SqlDeleteCompensacionManual, pecacodi, grupocodi,crdethora.ToString("dd/MM/yyyy HH:mm:ss"));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }


        public void DeleteByVersion(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteByVersion, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersionTipoCalculoAutomatico(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteByVersionTipoCalculoAutomatico, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
        
        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveEntity(VceCompRegularDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveManual);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Crdethora, DbType.DateTime, entity.Crdethora);
            dbProvider.AddInParameter(command, helper.Crdetvalor, DbType.Decimal, entity.Crdetvalor);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Crdetcvt, DbType.Decimal, entity.Crdetcvt);
            dbProvider.AddInParameter(command, helper.Crdetcmg, DbType.Decimal, entity.Crdetcmg);
            dbProvider.AddInParameter(command, helper.Crdetcompensacion, DbType.Decimal, entity.Crdetcompensacion);
            dbProvider.AddInParameter(command, helper.Crdetcvtbajaefic, DbType.Decimal, entity.Crdetcvtbajaefic);
            dbProvider.AddInParameter(command, helper.Crdettipocalc, DbType.String, entity.Crdettipocalc);

            dbProvider.ExecuteNonQuery(command);
        }

        public void LlenarMedeneriaGrupo(int pecacodi)
        {
            //Eliminamos VCE_COMP_REGULAR_DET para el periodo
            string query = "DELETE FROM VCE_COMP_REGULAR_DET WHERE pecacodi = " + pecacodi;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //FORMAMOS LA CONSULTA PARA EL REPORTE
            query = "INSERT INTO VCE_COMP_REGULAR_DET(pecacodi,GRUPOCODI,CRDETHORA,CRDETVALOR)";

            for (int i = 1; i <= 96; i++)
            {
                if (i > 1)
                {
                    query = query + " UNION ALL ";
                }
                query = query + " SELECT " + pecacodi + ",PTO.GRUPOCODI,ENE.CRMEFECHA + " + i + "/24/4 , SUM(CRMEH" + i + ")/4 AS VALOR ";
                query = query + " FROM (SELECT * FROM VCE_PTOMED_MODOPE WHERE PECACODI = " + pecacodi + " ) PTO JOIN VCE_ENERGIA ENE ON PTO.PTOMEDICODI = ENE.PTOMEDICODI AND ENE.pecacodi = " + pecacodi;
                query = query + " WHERE ENE.pecacodi = " + pecacodi;
                query = query + " GROUP BY PTO.GRUPOCODI,ENE.CRMEFECHA";
            }
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Ajustamos las fechas de inicio y fin de las horas de operación
            //query = "UPDATE VCE_COMP_REGULAR_DET SET SUBCAUSACODI = (SELECT MIN(SUBCAUSACODI) FROM VCE_HORA_OPERACION HOP WHERE VCE_COMP_REGULAR_DET.pecacodi=HOP.pecacodi AND VCE_COMP_REGULAR_DET.GRUPOCODI = HOP.GRUPOCODI AND VCE_COMP_REGULAR_DET.CRDETHORA >= (TO_DATE(TO_CHAR(CRHOPHORINI,'YYYY-MM-DD HH24'),'YYYY-MM-DD HH24')+ TRUNC(TO_CHAR(CRHOPHORINI,'MI')/15+1)*15*1/24/60) AND VCE_COMP_REGULAR_DET.CRDETHORA <= (TO_DATE(TO_CHAR(CRHOPHORFIN,'YYYY-MM-DD HH24'),'YYYY-MM-DD HH24')+ TRUNC(TO_CHAR(CRHOPHORFIN,'MI')/15+1)*15*1/24/60))";
            query = "UPDATE VCE_COMP_REGULAR_DET SET SUBCAUSACODI = (SELECT MIN(SUBCAUSACODI) FROM VCE_HORA_OPERACION HOP WHERE VCE_COMP_REGULAR_DET.pecacodi=HOP.pecacodi AND VCE_COMP_REGULAR_DET.GRUPOCODI = HOP.GRUPOCODI AND VCE_COMP_REGULAR_DET.CRDETHORA > CRHOPHORINI AND VCE_COMP_REGULAR_DET.CRDETHORA <= (TO_DATE(TO_CHAR(CRHOPHORFIN,'YYYY-MM-DD HH24'),'YYYY-MM-DD HH24')+ CASE WHEN TO_CHAR(CRHOPHORFIN,'MI')='00' THEN 0 ELSE TRUNC((TO_CHAR(CRHOPHORFIN,'MI')-1)/15+1)*15*1/24/60 END))";
            query = query + " WHERE pecacodi = " + pecacodi;
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos los periodos comunes en los rangos

            string queryHOComun = string.Format(helper.SqlListCursorHoraOperacionComun, pecacodi);
            DbCommand commandHOComun = dbProvider.GetSqlStringCommand(queryHOComun);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHOComun))
            {
                while (dr.Read())
                {
                    query = "UPDATE VCE_COMP_REGULAR_DET SET SUBCAUSACODI = " + dr["RANGO_SUBCAUSACODI"].ToString();
                    query = query + "WHERE pecacodi = " + pecacodi + " AND GRUPOCODI = " + dr["GRUPOCODI"].ToString() + " AND CRDETHORA = TO_DATE('" + dr["RANGO_HORA"].ToString() + "','DD/MM/YYYY HH24:MI:SS')";
                    command = dbProvider.GetSqlStringCommand(query);
                    dbProvider.ExecuteNonQuery(command);
                }
            }

            //Actualizamos las mediciones de energía para los periodos sin calificación
            query = "UPDATE VCE_COMP_REGULAR_DET SET CRDETVALOR = NULL WHERE pecacodi = " + pecacodi +" AND SUBCAUSACODI IS NULL AND CRDETVALOR IS NOT NULL";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Procesamos los modos con equipos en común
            query = "DELETE FROM TMP_GRUPOREL";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            query = "INSERT INTO TMP_GRUPOREL(GRUPOCODI,PTOMEDICODI,GRUPOCODIREL)";
            query = query + " SELECT G1.GRUPOCODI,G1.PTOMEDICODI,G2.GRUPOCODI AS GRUPOCODIREL";
            query = query + " FROM (SELECT * FROM VCE_PTOMED_MODOPE WHERE PECACODI = " + pecacodi + " ) G1 JOIN (SELECT * FROM VCE_PTOMED_MODOPE WHERE PECACODI = " + pecacodi + " ) G2 ON G1.PTOMEDICODI = G2.PTOMEDICODI";
            query = query + " WHERE G1.GRUPOCODI < G2.GRUPOCODI";
            query = query + " AND G1.GRUPOCODI IN (SELECT GRUPOCODI FROM VCE_HORA_OPERACION WHERE pecacodi = " + pecacodi + ")";
            query = query + "AND G2.GRUPOCODI IN (SELECT GRUPOCODI FROM VCE_HORA_OPERACION WHERE pecacodi = " + pecacodi + ")";
            query = query + "ORDER BY 1";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            string vGrupoCodiBase = "";
            string vFechaEvaluacion = "";

            string queryGrupoComun = string.Format(helper.SqlListCursorGrupoComun);
            DbCommand commandGrupoComun = dbProvider.GetSqlStringCommand(queryGrupoComun);
            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupoComun))
            {
                while (dr.Read())
                {
                    vGrupoCodiBase = dr["GRUPOCODI"].ToString();

                    string queryFechasGrupoComun = string.Format(helper.SqlListCursorFechasComun, pecacodi, vGrupoCodiBase);
                    DbCommand commandFechasComun = dbProvider.GetSqlStringCommand(queryFechasGrupoComun);

                    using (IDataReader drFecha = dbProvider.ExecuteReader(commandFechasComun))
                    {
                        while (drFecha.Read())
                        {
                            vFechaEvaluacion = drFecha["CRDETHORA"].ToString();

                            string queryParticipacionComun = string.Format(helper.SqlListCursorParticipacionComun, pecacodi, vGrupoCodiBase, vFechaEvaluacion);
                            DbCommand commandParticipacionComun = dbProvider.GetSqlStringCommand(queryParticipacionComun);

                            using (IDataReader drParticipacion = dbProvider.ExecuteReader(commandParticipacionComun))
                            {
                                while (drParticipacion.Read())
                                {
                                    query = "UPDATE VCE_COMP_REGULAR_DET SET SUBCAUSACODI = CASE WHEN GRUPOCODI = " + drParticipacion["GRUPOCODI"].ToString() + " THEN SUBCAUSACODI ELSE NULL END";
                                    query = query + " WHERE pecacodi = " + pecacodi;
                                    query = query + " AND CRDETHORA = TO_DATE('" + vFechaEvaluacion + "','DD/MM/YYYY HH24:MI:SS')";
                                    query = query + " AND (GRUPOCODI = " + vGrupoCodiBase + " OR GRUPOCODI IN (SELECT GRUPOCODIREL FROM TMP_GRUPOREL WHERE GRUPOCODI = " + vGrupoCodiBase + " ))";
                                    command = dbProvider.GetSqlStringCommand(query);
                                    dbProvider.ExecuteNonQuery(command);
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            //Inicializamos los puntos que cambiaron de calificación
            query = string.Format(helper.SqlInicializarEnergiaSinCalificacion, pecacodi);
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

        }

    }

}

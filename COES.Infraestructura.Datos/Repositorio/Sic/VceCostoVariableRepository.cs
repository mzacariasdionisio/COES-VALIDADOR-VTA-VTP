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
    /// Clase de acceso a datos de la tabla EVE_SUBCAUSAEVENTO
    /// </summary>
    public class VceCostoVariableRepository: RepositoryBase, IVceCostoVariableRepository
    {
        public VceCostoVariableRepository(string strConn) : base(strConn)
        {
        }

        VceCostoVariableHelper helper = new VceCostoVariableHelper();

        public void Save(VceCostoVariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crcvcvtbajaefic, DbType.Decimal, entity.Crcvcvtbajaefic);
            dbProvider.AddInParameter(command, helper.Crcvcvcbajaefic, DbType.Decimal, entity.Crcvcvcbajaefic);
            dbProvider.AddInParameter(command, helper.Crcvconsumobajaefic, DbType.Decimal, entity.Crcvconsumobajaefic);
            dbProvider.AddInParameter(command, helper.Crcvpotenciabajaefic, DbType.Decimal, entity.Crcvpotenciabajaefic);
            dbProvider.AddInParameter(command, helper.Crcvaplicefectiva, DbType.String, entity.Crcvaplicefectiva);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Crcvcvt, DbType.Decimal, entity.Crcvcvt);
            dbProvider.AddInParameter(command, helper.Crcvcvnc, DbType.Decimal, entity.Crcvcvnc);
            dbProvider.AddInParameter(command, helper.Crcvcvc, DbType.Decimal, entity.Crcvcvc);
            dbProvider.AddInParameter(command, helper.Crcvprecioaplic, DbType.Decimal, entity.Crcvprecioaplic);
            dbProvider.AddInParameter(command, helper.Crcvconsumo, DbType.Decimal, entity.Crcvconsumo);
            dbProvider.AddInParameter(command, helper.Crcvpotencia, DbType.Decimal, entity.Crcvpotencia);
            dbProvider.AddInParameter(command, helper.Crcvfecmod, DbType.DateTime, entity.Crcvfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceCostoVariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crcvcvtbajaefic, DbType.Decimal, entity.Crcvcvtbajaefic);
            dbProvider.AddInParameter(command, helper.Crcvcvcbajaefic, DbType.Decimal, entity.Crcvcvcbajaefic);
            dbProvider.AddInParameter(command, helper.Crcvconsumobajaefic, DbType.Decimal, entity.Crcvconsumobajaefic);
            dbProvider.AddInParameter(command, helper.Crcvpotenciabajaefic, DbType.Decimal, entity.Crcvpotenciabajaefic);
            dbProvider.AddInParameter(command, helper.Crcvaplicefectiva, DbType.String, entity.Crcvaplicefectiva);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Crcvcvt, DbType.Decimal, entity.Crcvcvt);
            dbProvider.AddInParameter(command, helper.Crcvcvnc, DbType.Decimal, entity.Crcvcvnc);
            dbProvider.AddInParameter(command, helper.Crcvcvc, DbType.Decimal, entity.Crcvcvc);
            dbProvider.AddInParameter(command, helper.Crcvprecioaplic, DbType.Decimal, entity.Crcvprecioaplic);
            dbProvider.AddInParameter(command, helper.Crcvconsumo, DbType.Decimal, entity.Crcvconsumo);
            dbProvider.AddInParameter(command, helper.Crcvpotencia, DbType.Decimal, entity.Crcvpotencia);
            dbProvider.AddInParameter(command, helper.Crcvfecmod, DbType.DateTime, entity.Crcvfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime crcvfecmod, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crcvfecmod, DbType.DateTime, crcvfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceCostoVariableDTO GetById(DateTime crcvfecmod, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crcvfecmod, DbType.DateTime, crcvfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            VceCostoVariableDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceCostoVariableDTO> List()
        {
            List<VceCostoVariableDTO> entitys = new List<VceCostoVariableDTO>();
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

        public List<VceCostoVariableDTO> GetByCriteria()
        {
            List<VceCostoVariableDTO> entitys = new List<VceCostoVariableDTO>();
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

        //NETC

        public List<VceCostoVariableDTO> ListCostoVariable(int pecacodi)
        {
            List<VceCostoVariableDTO> entitys = new List<VceCostoVariableDTO>();

            string query = string.Format(helper.SqlListCostoVariable, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            VceCostoVariableDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new VceCostoVariableDTO();

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int iCrcvPotencia = dr.GetOrdinal(helper.Crcvpotencia);
                    if (!dr.IsDBNull(iCrcvPotencia)) entity.Crcvpotencia = dr.GetDecimal(iCrcvPotencia);

                    int iCrcvConsumo = dr.GetOrdinal(helper.Crcvconsumo);
                    if (!dr.IsDBNull(iCrcvConsumo)) entity.Crcvconsumo = dr.GetDecimal(iCrcvConsumo);

                    int iCrcvPrecioAplic = dr.GetOrdinal(helper.Crcvprecioaplic);
                    if (!dr.IsDBNull(iCrcvPrecioAplic)) entity.Crcvprecioaplic = dr.GetDecimal(iCrcvPrecioAplic);

                    int iCrdcgPrecioAplicUnid = dr.GetOrdinal(helper.Crdcgprecioaplicunid);
                    if (!dr.IsDBNull(iCrdcgPrecioAplicUnid)) entity.Crdcgprecioaplicunid = dr.GetString(iCrdcgPrecioAplicUnid);

                    int iCrcvCvc = dr.GetOrdinal(helper.Crcvcvc);
                    if (!dr.IsDBNull(iCrcvCvc)) entity.Crcvcvc = dr.GetDecimal(iCrcvCvc);

                    int iCrcvCvnc = dr.GetOrdinal(helper.Crcvcvnc);
                    if (!dr.IsDBNull(iCrcvCvnc)) entity.Crcvcvnc = dr.GetDecimal(iCrcvCvnc);

                    int iCrcvCvt = dr.GetOrdinal(helper.Crcvcvt);
                    if (!dr.IsDBNull(iCrcvCvt)) entity.Crcvcvt = dr.GetDecimal(iCrcvCvt);

                    int iBarrBarraTransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrBarraTransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrBarraTransferencia);

                    int iCrcvAplicEfectiva = dr.GetOrdinal(helper.Crcvaplicefectiva);
                    if (!dr.IsDBNull(iCrcvAplicEfectiva)) entity.Crcvaplicefectiva = dr.GetString(iCrcvAplicEfectiva);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public void LlenarCostoVariable(int pecacodi)
        {
            //Eliminamos los Costos Variables para el periodo
            string query = "DELETE FROM VCE_COSTO_VARIABLE WHERE pecacodi = " + pecacodi;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Insertamos los costos variables para el periodo
            query = "INSERT INTO VCE_COSTO_VARIABLE(pecacodi,GRUPOCODI,CRCVFECMOD,BARRCODI) ";
            query = query + " SELECT CA.pecacodi,CA.GRUPOCODI,CA.CRDCGFECMOD,CA.BARRCODI ";
            query = query + " FROM VCE_DATCALCULO CA WHERE CA.pecacodi = " + pecacodi;
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el flag de aplicación de potencia efectiva
            query = "UPDATE VCE_COSTO_VARIABLE SET CRCVAPLICEFECTIVA ='Si'";
            query = query + " WHERE pecacodi = " + pecacodi;
            query = query + " AND (GRUPOCODI NOT IN (SELECT GRUPOCODI FROM VCE_COMP_REGULAR_DET WHERE pecacodi = " + pecacodi + " AND SUBCAUSACODI = 101)";
            query = query + " OR GRUPOCODI IN (SELECT GRUPOCODI FROM VCE_DATCALCULO CA WHERE pecacodi = " + pecacodi + " AND CRDCGCONSIDERAPOTNOM=1))";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos las potencias
            //query = "UPDATE VCE_COSTO_VARIABLE SET (CRCVPOTENCIA) = (SELECT MAX(VCE_COMPENSACION_PKG.FNC_CALC_POTENCIA_MODOPE(DC.pecacodi, DC.GRUPOCODI, 'SI','PE',DC.CRDCGPOTMIN, DC.CRDCGPOTEFE))";
            //query = query + " FROM VCE_DATCALCULO DC ";
            //query = query + " WHERE DC.pecacodi = VCE_COSTO_VARIABLE.pecacodi AND DC.GRUPOCODI = VCE_COSTO_VARIABLE.GRUPOCODI)";
            //query = query + " WHERE pecacodi = " + pecacodi;
            //command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            // DSH 05-06-2017 ; Se cambio por requerimiento
            query = "DECLARE";
            query = query + " CURSOR CUR_CV IS";
            query = query + " SELECT CV.PECACODI, CV.GRUPOCODI, CV.CRCVPOTENCIA, 'SI' AJUSTE_POT_MIN, 'PE' TIPO, DC.CRDCGPOTMIN POTMIN, DC.CRDCGPOTEFE POTMAX";
            query = query + " FROM VCE_COSTO_VARIABLE CV";
            query = query + " INNER JOIN VCE_DATCALCULO DC ON CV.PECACODI = DC.PECACODI AND CV.GRUPOCODI = DC.GRUPOCODI";
            query = query + " WHERE CV.PECACODI = " + pecacodi;
            query = query + " FOR UPDATE OF CV.CRCVPOTENCIA;";
            query = query + " CUR_CV_REC CUR_CV%ROWTYPE;";
            query = query + " V_ENERGIA NUMBER;";
            query = query + " V_NRO_PER INTEGER;";
            query = query + " V_DESCUENTO_ENERGIA NUMBER;";
            query = query + " V_DESCUENTO_TIEMPO NUMBER;";
            query = query + " V_SUBCAUSACODI INTEGER := 101;";
            query = query + " V_POTENCIA NUMBER;";
            query = query + " BEGIN";
            query = query + "  FOR CUR_CV_REC IN CUR_CV LOOP";
            query = query + "   V_ENERGIA := 0;";
            query = query + "   V_NRO_PER := 0;";
            query = query + "   V_POTENCIA := NULL;";
            query = query + "   V_DESCUENTO_ENERGIA :=0;";
            query = query + "   V_DESCUENTO_TIEMPO := 0;";
            query = query + "   SELECT SUM(CRDETVALOR),COUNT(*) INTO V_ENERGIA, V_NRO_PER ";
            query = query + "   FROM VCE_COMP_REGULAR_DET";
            query = query + "   WHERE PECACODI = CUR_CV_REC.PECACODI AND GRUPOCODI = CUR_CV_REC.GRUPOCODI AND SUBCAUSACODI= V_SUBCAUSACODI;";
            query = query + "   IF CUR_CV_REC.TIPO = 'PE' THEN";
            query = query + "    SELECT NVL(SUM(DC.CRDCGNUMARRPAR*DC.CRDCGENERGIA),0), NVL(SUM(DC.CRDCGNUMARRPAR*DC.CRDCGTIEMPO),0)";
            query = query + "    INTO V_DESCUENTO_ENERGIA,V_DESCUENTO_TIEMPO";
            query = query + "    FROM VCE_DATCALCULO DC";
            query = query + "    WHERE DC.PECACODI = CUR_CV_REC.PECACODI AND DC.GRUPOCODI = CUR_CV_REC.GRUPOCODI AND TO_CHAR(DC.CRDCGFECMOD,'DD')='01';";
            query = query + "   END IF;";
            query = query + "   V_POTENCIA := (V_ENERGIA - V_DESCUENTO_ENERGIA) / (V_NRO_PER * 0.25 - V_DESCUENTO_TIEMPO);";
            query = query + "   IF UPPER(CUR_CV_REC.AJUSTE_POT_MIN) ='SI' THEN";
            query = query + "    IF V_POTENCIA < CUR_CV_REC.POTMIN THEN";
            query = query + "      V_POTENCIA := CUR_CV_REC.POTMIN;";
            query = query + "    ELSIF V_POTENCIA > CUR_CV_REC.POTMAX THEN";
            query = query + "      V_POTENCIA := CUR_CV_REC.POTMAX;";
            query = query + "    END IF;";
            query = query + "   END IF;";
            query = query + "   UPDATE VCE_COSTO_VARIABLE CV";
            query = query + "   SET CV.CRCVPOTENCIA = V_POTENCIA";
            query = query + "   WHERE CURRENT OF CUR_CV;";
            query = query + "  END LOOP;";
            query = query + " END;";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el consumo de combustible
            //DSH 02-06-2017 : Se cambio por requerimiento

            /* query = "UPDATE VCE_COSTO_VARIABLE SET (CRCVCONSUMO) = (SELECT MAX(VCE_COMPENSACION_PKG.FNC_CALC_CONSUMO_COMB(VCE_COSTO_VARIABLE.CRCVPOTENCIA, DC.CRDCGPOTEFE,DC.CRDCGCCPOTEFE,DC.CRDCGPOTPAR1,DC.CRDCGCONCOMPP1,DC.CRDCGPOTPAR2,DC.CRDCGCONCOMPP2,DC.CRDCGPOTPAR3,DC.CRDCGCONCOMPP3,DC.CRDCGPOTPAR4,DC.CRDCGCONCOMPP4 ))";
             query = query + " FROM VCE_DATCALCULO DC ";
             query = query + " WHERE DC.pecacodi = VCE_COSTO_VARIABLE.pecacodi AND DC.GRUPOCODI = VCE_COSTO_VARIABLE.GRUPOCODI";
             query = query + " AND UPPER(NVL(CRCVAPLICEFECTIVA,'NO')) <>'SI')";
             query = query + " WHERE pecacodi = " + pecacodi;
            */

            query = "DECLARE CURSOR CV_CUR IS";
            query = query + " SELECT CV.CRCVCONSUMO AS CRCVCONSUMO, CV.CRCVPOTENCIA AS POTENCIA, DC.CRDCGPOTEFE, DC.CRDCGCCPOTEFE,DC.CRDCGPOTPAR1,DC.CRDCGCONCOMPP1,DC.CRDCGPOTPAR2,";
            query = query + " DC.CRDCGCONCOMPP2,DC.CRDCGPOTPAR3,DC.CRDCGCONCOMPP3,DC.CRDCGPOTPAR4,DC.CRDCGCONCOMPP4, CV.ROWID";
            query = query + " FROM VCE_COSTO_VARIABLE CV INNER JOIN VCE_DATCALCULO DC ON CV.PECACODI = DC.PECACODI AND CV.GRUPOCODI = DC.GRUPOCODI AND CV.CRCVFECMOD = DC.CRDCGFECMOD";
            query = query + " WHERE CV.PECACODI = " + pecacodi + " AND  UPPER(NVL(CV.CRCVAPLICEFECTIVA,'NO')) <>'SI'";
            query = query + " FOR UPDATE OF CV.CRCVCONSUMO;";
            query = query + " V_CONSUMO number;";
            query = query + " BEGIN";
            query = query + " FOR CV_ROW IN CV_CUR LOOP";
            query = query + " V_CONSUMO := NULL;";
            query = query + " IF (NVL(CV_ROW.Crdcgpotpar1 ,0) > 0 AND (CV_ROW.Crdcgpotpar1  < CV_ROW.potencia) AND (CV_ROW.potencia <= CV_ROW.Crdcgpotefe))THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp1 + (CV_ROW.potencia - CV_ROW.Crdcgpotpar1 ) * (CV_ROW.Crdcgccpotefe - CV_ROW.Crdcgconcompp1) / (CV_ROW.Crdcgpotefe - CV_ROW.Crdcgpotpar1 );";
            query = query + " END IF;";
            query = query + " IF (NVL(CV_ROW.Crdcgpotpar2,0) > 0 AND (CV_ROW.Crdcgpotpar2 < CV_ROW.potencia) AND (CV_ROW.potencia <= CV_ROW.Crdcgpotpar1 )) THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp2 + (CV_ROW.potencia - CV_ROW.Crdcgpotpar2) * (CV_ROW.Crdcgconcompp1 - CV_ROW.Crdcgconcompp2) / (CV_ROW.Crdcgpotpar1  - CV_ROW.Crdcgpotpar2);";
            query = query + " END IF;";
            query = query + " IF (NVL(CV_ROW.Crdcgpotpar3,0) > 0 AND (CV_ROW.Crdcgpotpar3 < CV_ROW.potencia) AND (CV_ROW.potencia <= CV_ROW.Crdcgpotpar2)) THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp3 + (CV_ROW.potencia - CV_ROW.Crdcgpotpar3) * (CV_ROW.Crdcgconcompp2 - CV_ROW.Crdcgconcompp3) / (CV_ROW.Crdcgpotpar2 - CV_ROW.Crdcgpotpar3);";
            query = query + " END IF;";
            query = query + " IF (NVL(CV_ROW.Crdcgpotpar4,0) > 0 AND (CV_ROW.Crdcgpotpar4 <= CV_ROW.potencia) AND (CV_ROW.potencia <= CV_ROW.Crdcgpotpar3)) THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp4 + (CV_ROW.potencia -  CV_ROW.Crdcgpotpar4) * (CV_ROW.Crdcgconcompp3 - CV_ROW.Crdcgconcompp4) / (CV_ROW.Crdcgpotpar3 - CV_ROW.Crdcgpotpar4);";
            query = query + " END IF;";
            query = query + " IF V_CONSUMO IS NULL THEN";
            query = query + " IF NVL(CV_ROW.Crdcgpotpar4,0) > 0 THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp4 + (CV_ROW.potencia -  CV_ROW.Crdcgpotpar4) * (CV_ROW.Crdcgconcompp3 - CV_ROW.Crdcgconcompp4) / (CV_ROW.Crdcgpotpar3 - CV_ROW.Crdcgpotpar4);";
            query = query + " ELSIF NVL(CV_ROW.Crdcgpotpar3,0) > 0 THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp3 + (CV_ROW.potencia - CV_ROW.Crdcgpotpar3) * (CV_ROW.Crdcgconcompp2 - CV_ROW.Crdcgconcompp3) / (CV_ROW.Crdcgpotpar2 - CV_ROW.Crdcgpotpar3);";
            query = query + " ELSIF NVL(CV_ROW.Crdcgpotpar2,0) > 0  THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp2 + (CV_ROW.potencia - CV_ROW.Crdcgpotpar2) * (CV_ROW.Crdcgconcompp1 - CV_ROW.Crdcgconcompp2) / (CV_ROW.Crdcgpotpar1  - CV_ROW.Crdcgpotpar2);";
            query = query + " ELSIF NVL(CV_ROW.Crdcgpotpar1 ,0) > 0 THEN";
            query = query + " V_CONSUMO := CV_ROW.Crdcgconcompp1 + (CV_ROW.potencia - CV_ROW.Crdcgpotpar1 ) * (CV_ROW.Crdcgccpotefe - CV_ROW.Crdcgconcompp1) / (CV_ROW.Crdcgpotefe - CV_ROW.Crdcgpotpar1 );";
            query = query + " END IF;";
            query = query + "END IF;";
            query = query + " UPDATE VCE_COSTO_VARIABLE";
            query = query + " SET CRCVCONSUMO = V_CONSUMO";
            query = query + " WHERE ROWID = CV_ROW.ROWID;";
            query = query + " END LOOP;";
            query = query + " END;";

            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Completamos los datos para los que no operaron
            query = "UPDATE VCE_COSTO_VARIABLE SET (CRCVPOTENCIA,CRCVCONSUMO) = (SELECT CRDCGPOTEFE,CRDCGCCPOTEFE";
            query = query + "   FROM VCE_DATCALCULO DC ";
            query = query + "   WHERE DC.pecacodi = VCE_COSTO_VARIABLE.pecacodi AND DC.GRUPOCODI = VCE_COSTO_VARIABLE.GRUPOCODI";
            query = query + "   AND DC.CRDCGFECMOD = VCE_COSTO_VARIABLE.CRCVFECMOD)";
            query = query + " WHERE pecacodi = " + pecacodi;
            query = query + " AND UPPER(CRCVAPLICEFECTIVA)='SI'";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el precio a aplicar
            query = "UPDATE VCE_COSTO_VARIABLE SET CRCVPRECIOAPLIC = (SELECT DC.CRDCGPRECIOAPLIC ";
            query = query + "   FROM VCE_DATCALCULO DC ";
            query = query + "   WHERE DC.pecacodi = VCE_COSTO_VARIABLE.pecacodi AND DC.GRUPOCODI = VCE_COSTO_VARIABLE.GRUPOCODI AND DC.CRDCGFECMOD = VCE_COSTO_VARIABLE.CRCVFECMOD)";
            query = query + " WHERE pecacodi = " + pecacodi;
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el CVC
            //query = "UPDATE VCE_COSTO_VARIABLE SET (CRCVCVC) = (SELECT MAX(VCE_COMPENSACION_PKG.FNC_CALC_CVC(DC.pecacodi, DC.GRUPOCODI, VCE_COSTO_VARIABLE.CRCVCONSUMO, VCE_COSTO_VARIABLE.CRCVPOTENCIA, VCE_COSTO_VARIABLE.CRCVPRECIOAPLIC, PE.PECATIPOCAMBIO))";
            //query = query + "   FROM VCE_DATCALCULO DC JOIN VCE_PERIODO_CALCULO PE ON DC.PECACODI = PE.PECACODI";
            //query = query + "   WHERE DC.pecacodi = VCE_COSTO_VARIABLE.pecacodi AND DC.GRUPOCODI = VCE_COSTO_VARIABLE.GRUPOCODI)";
            //query = query + " WHERE pecacodi = " + pecacodi;


            // DSH 05-06-2017 - Se cambio por requerimiento
            query = "DECLARE";
            query = query + " CURSOR CUR_CV IS";
            query = query + " SELECT CV.PECACODI, CV.GRUPOCODI, CV.CRCVPOTENCIA POTENCIA,  CV.CRCVCONSUMO CONSUMO,";
            query = query + " CV.CRCVPRECIOAPLIC PRECIOAPLIC, CV.CRCVCVC CVC, PE.PECATIPOCAMBIO";
            query = query + " FROM VCE_COSTO_VARIABLE CV";
            query = query + " INNER JOIN VCE_DATCALCULO DC ON CV.PECACODI = DC.PECACODI AND CV.GRUPOCODI = DC.GRUPOCODI";
            query = query + " JOIN VCE_PERIODO_CALCULO PE ON DC.PECACODI = PE.PECACODI";
            query = query + " WHERE CV.PECACODI = " + pecacodi;
            query = query + " FOR UPDATE OF CV.CRCVCVC;";
            query = query + " CURSOR CUR_DATCAL(V_PECACODI IN NUMBER, V_GRUPOCODI IN NUMBER) IS";
            query = query + " SELECT GR.FENERGCODI,DC.CRDCGLHV";
            query = query + " FROM PR_GRUPO GR JOIN VCE_DATCALCULO DC ON GR.GRUPOCODI = DC.GRUPOCODI";
            query = query + " WHERE DC.PECACODI = V_PECACODI AND DC.GRUPOCODI = V_GRUPOCODI";
            query = query + " AND TO_CHAR(DC.CRDCGFECMOD,'DD')='01';";
            query = query + " CUR_CV_REC CUR_CV%ROWTYPE;";
            query = query + " CUR_DATCAL_REC CUR_DATCAL%ROWTYPE;";
            query = query + " V_CVC NUMBER;";
            query = query + " BEGIN";
            query = query + " FOR CUR_CV_REC IN CUR_CV LOOP";
            query = query + "  V_CVC := NULL;";
            query = query + "  FOR CUR_DATCAL_REC IN CUR_DATCAL(CUR_CV_REC.PECACODI, CUR_CV_REC.GRUPOCODI) LOOP";
            query = query + "   IF CUR_CV_REC.POTENCIA <>0 THEN";
            query = query + "     IF CUR_DATCAL_REC.FENERGCODI IN (3,4) THEN";
            query = query + "       V_CVC := CUR_CV_REC.CONSUMO * CUR_CV_REC.PRECIOAPLIC / (CUR_CV_REC.POTENCIA * 1000);";
            query = query + "     ELSIF CUR_DATCAL_REC.FENERGCODI IN (5) THEN";
            query = query + "       V_CVC := CUR_CV_REC.CONSUMO * CUR_CV_REC.PRECIOAPLIC / (CUR_CV_REC.POTENCIA * 1000);";
            query = query + "     ELSIF CUR_DATCAL_REC.FENERGCODI IN (2) THEN";
            query = query + "       V_CVC := CUR_CV_REC.CONSUMO * CUR_CV_REC.PRECIOAPLIC  * (NVL(CUR_DATCAL_REC.CRDCGLHV,0) / 1000) / (CUR_CV_REC.POTENCIA * 1000000);";
            query = query + "     ELSE";
            query = query + "       V_CVC := 0;";
            query = query + "     END IF;";
            query = query + "   END IF;";
            query = query + "  END LOOP;";
            query = query + "  UPDATE VCE_COSTO_VARIABLE CV";
            query = query + "  SET CV.CRCVCVC = V_CVC";
            query = query + "  WHERE CURRENT OF CUR_CV;";
            query = query + " END LOOP;";
            query = query + " END;";
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el CVNC
            query = "UPDATE VCE_COSTO_VARIABLE SET CRCVCVNC = (SELECT MAX(NVL(DC.CRDCGCVNCDOL*PE.PECATIPOCAMBIO,0)+NVL(DC.CRDCGCVNCSOL,0))";
            query = query + "   FROM VCE_DATCALCULO DC JOIN VCE_PERIODO_CALCULO PE ON DC.PECACODI = PE.PECACODI";
            query = query + "   WHERE DC.pecacodi = VCE_COSTO_VARIABLE.pecacodi AND DC.GRUPOCODI = VCE_COSTO_VARIABLE.GRUPOCODI)";
            query = query + " WHERE pecacodi = " + pecacodi;
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos el CVNC
            query = "UPDATE VCE_COSTO_VARIABLE SET CRCVCVT = NVL(CRCVCVC,0)+NVL(CRCVCVNC,0)";
            query = query + " WHERE pecacodi = " + pecacodi;
            command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersion(int pecacodi)
        {
            try
            {
                string queryString = string.Format(helper.SqlDeleteByVersion, pecacodi);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}

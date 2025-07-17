using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class DatosTransferenciaRepository : RepositoryBase
    {

        public DatosTransferenciaRepository(string strConn) : base(strConn)
        {
        }

        DatosTransferenciaHelper helper = new DatosTransferenciaHelper();

        public List<DatosTransferencia> SaveEntregaRetiro(List<DatosTransferencia> entitys, int periodo, int version, int empresa, string usuario, int trnenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete); //TRN_ENTREGA_RETIRO_TEMP
            command.CommandTimeout = 0;
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();

            for(int i=0;i< entitys.Count; i++)
            {
                entitys[i].Promedio = Math.Round(Convert.ToDecimal(entitys[i].Promedio), 10);
                entitys[i].Sumadia = Math.Round(Convert.ToDecimal(entitys[i].Sumadia), 10);
            }

            #region AddColumnMapping
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Nrodia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Tipodato, DbType.String);
            dbProvider.AddColumnMapping(helper.Codigobarra, DbType.String);
            dbProvider.AddColumnMapping(helper.Promedio, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Sumadia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H59, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H60, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H61, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H62, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H63, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H64, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H65, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H66, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H67, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H68, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H69, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H70, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H71, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H72, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H73, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H74, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H75, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H76, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H77, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H78, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H79, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H80, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H81, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H82, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H83, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H84, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H85, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H86, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H87, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H88, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H89, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H90, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H91, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H92, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H93, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H94, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H95, DbType.Decimal);
            dbProvider.AddColumnMapping( helper.H96, DbType.Decimal);
#endregion

            dbProvider.BulkInsert<DatosTransferencia>(entitys, helper.NombreTabla);
            string sql = string.Format(helper.SqlProcesarInformacionEnvio, empresa, periodo, version, usuario, trnenvcodi, "ACT");
            command.Parameters.Clear();
            command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);

            List<DatosTransferencia> resultado = new List<DatosTransferencia>();

            command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCodigos);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DatosTransferencia entity = new DatosTransferencia();

                    int iCodigo = dr.GetOrdinal(helper.Codigobarra);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigobarra = dr.GetString(iCodigo);

                    int iIndbarra = dr.GetOrdinal(helper.Indbarra);
                    if (!dr.IsDBNull(iIndbarra)) entity.Indbarra = dr.GetString(iIndbarra);

                    resultado.Add(entity);
                }
            }

            return resultado;
        }

        //ASSETEC 202001
        public List<DatosTransferencia> SaveEntregaRetiroEnvio(List<DatosTransferencia> entitys, int periodo, int version, int empresa, string usuario, int trnenvcodi, string testado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete); //TRN_ENTREGA_RETIRO_TEMP
            command.CommandTimeout = 0;
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();

            if (testado.Equals("ACT"))
            {
                //Actualizamos todos los envios anteriores a desactualizados
                string sqlUpdate = string.Format(helper.SqlUpdate, periodo, version, empresa, usuario); //CAMBIAMOS EL ESTADO DE LAS ENTREGAS Y RETIROS A INACTIVO
                command = dbProvider.GetSqlStringCommand(sqlUpdate); 
                command.CommandTimeout = 0;
                dbProvider.ExecuteNonQuery(command);
                command.Parameters.Clear();
            }
            #region AddColumnMapping
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Nrodia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Tipodato, DbType.String);
            dbProvider.AddColumnMapping(helper.Codigobarra, DbType.String);
            dbProvider.AddColumnMapping(helper.Promedio, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Sumadia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H96, DbType.Decimal);
            #endregion

            dbProvider.BulkInsert<DatosTransferencia>(entitys, helper.NombreTabla);
            string sql = string.Format(helper.SqlProcesarInformacionEnvio, empresa, periodo, version, usuario, trnenvcodi, testado);
            command.Parameters.Clear();
            command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);

            List<DatosTransferencia> resultado = new List<DatosTransferencia>();

            command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCodigos);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DatosTransferencia entity = new DatosTransferencia();

                    int iCodigo = dr.GetOrdinal(helper.Codigobarra);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigobarra = dr.GetString(iCodigo);

                    int iIndbarra = dr.GetOrdinal(helper.Indbarra);
                    if (!dr.IsDBNull(iIndbarra)) entity.Indbarra = dr.GetString(iIndbarra);

                    resultado.Add(entity);
                }
            }

            return resultado;
        }

        public List<DatosTransferencia> SaveModeloEnvio(List<DatosTransferencia> entitys, int periodo, int version, string listaEmpresas, string usuario, int trnenvcodi, int trnmodcodi, string testado)
        {
            string sql = string.Format(helper.SqlDeleteListaEmpresas, listaEmpresas); //TRN_ENTREGA_RETIRO_TEMP
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();

            #region AddColumnMapping
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Nrodia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Tipodato, DbType.String);
            dbProvider.AddColumnMapping(helper.Codigobarra, DbType.String);
            dbProvider.AddColumnMapping(helper.Promedio, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Sumadia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H96, DbType.Decimal);
            #endregion

            dbProvider.BulkInsert<DatosTransferencia>(entitys, helper.NombreTabla);
            sql = string.Format(helper.SqlProcesarInformacionModeloEnvio, listaEmpresas, periodo, version, usuario, trnenvcodi, testado, trnmodcodi);
            command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);

            List<DatosTransferencia> resultado = new List<DatosTransferencia>();

            sql = string.Format(helper.SqlObtenerCodigosModelo, listaEmpresas);
            command = dbProvider.GetSqlStringCommand(sql); 

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DatosTransferencia entity = new DatosTransferencia();

                    int iCodigo = dr.GetOrdinal(helper.Codigobarra);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigobarra = dr.GetString(iCodigo);

                    int iIndbarra = dr.GetOrdinal(helper.Indbarra);
                    if (!dr.IsDBNull(iIndbarra)) entity.Indbarra = dr.GetString(iIndbarra);

                    resultado.Add(entity);
                }
            }

            return resultado;
        }
        public void UpdateRetirosInactivo(int trnenvcodi, int periodo, int version)
        {
            String sql = string.Format(helper.SqlUpdateRetirosInactivo, trnenvcodi, periodo, version);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
        }

    }
}

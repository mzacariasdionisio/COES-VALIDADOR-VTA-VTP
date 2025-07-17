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
    public class MeMedicion96Repository : RepositoryBase, IMeMedicion96Repository
    {
        public MeMedicion96Repository(string strConn)
            : base(strConn)
        {
        }

        MeMedicion96Helper helper = new MeMedicion96Helper();

        public void Save(MeMedicion96DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarMedicion(int lectcodi, DateTime fechahora, int tipoinfocodi, int ptomedicodi, int h, decimal? valor)
        {
            string sql = string.Format(helper.SqlActualizarValor, h, valor, lectcodi, fechahora.ToString(ConstantesBase.FormatoFecha),
                tipoinfocodi, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion96DTO> ObtenerConsultaMedidores(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int famcodiSSAA, string tipogrupocodiNoIntegrante
            , int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int nroPagina, int nroRegistros, int tipoptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaTipoGeneracion, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa
                , lectcodi, tipoinfocodi, tipogrupocodiRer, nroPagina, nroRegistros, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<MeMedicion96DTO> ObtenerConsultaServiciosAuxiliares(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int famcodiSSAA, string tipogrupocodiNoIntegrante
            , int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int nroPagina, int nroRegistros, int tipoptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaServiciosAuxiliares, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa
                , lectcodi, tipoinfocodi, tipogrupocodiRer, nroPagina, nroRegistros, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerExportacionConsultaMedidores(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string sql = String.Format(this.helper.SqlObtenerExportacionConsultaMedidores, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    if (string.IsNullOrEmpty(entity.Central)) entity.Central = string.Empty;
                    if (string.IsNullOrEmpty(entity.Equinomb)) entity.Equinomb = string.Empty;
                    if (string.IsNullOrEmpty(entity.Ptomedielenomb)) entity.Equinomb = string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerExportacionServiciosAuxiliares(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string sql = String.Format(this.helper.SqlObtenerExportacionServiciosAuxiliares, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    if (string.IsNullOrEmpty(entity.Central)) entity.Central = string.Empty;
                    if (string.IsNullOrEmpty(entity.Equinomb)) entity.Equinomb = string.Empty;
                    if (string.IsNullOrEmpty(entity.Ptomedielenomb)) entity.Equinomb = string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerTotalConsultaMedidores(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string query = String.Format(helper.SqlTotalConsultaMedidores, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    #region Lectura

                    MeMedicion96DTO suma = new MeMedicion96DTO();
                    MeMedicion96DTO maximo = new MeMedicion96DTO();
                    MeMedicion96DTO minimo = new MeMedicion96DTO();

                    //int iMeditotal = dr.GetOrdinal(this.helper.Meditotal);
                    //if (!dr.IsDBNull(iMeditotal)) suma.Meditotal = dr.GetDecimal(iMeditotal) / 4;
                    //else suma.Meditotal = 0;

                    decimal maxValue = decimal.MinValue;
                    decimal minValue = decimal.MaxValue;
                    decimal totalValue = 0;

                    for (int i = 1; i <= 96; i++)
                    {
                        decimal valorSuma = 0;
                        decimal valorMaximo = 0;
                        decimal valorMinimo = 0;

                        int iSuma = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iSuma)) valorSuma = dr.GetDecimal(iSuma) / 4;

                        int iMaximo = dr.GetOrdinal("G" + i);
                        if (!dr.IsDBNull(iMaximo)) valorMaximo = dr.GetDecimal(iMaximo);

                        int iMinimo = dr.GetOrdinal("M" + i);
                        if (!dr.IsDBNull(iMinimo)) valorMinimo = dr.GetDecimal(iMinimo);

                        suma.GetType().GetProperty("H" + i).SetValue(suma, valorSuma);
                        maximo.GetType().GetProperty("H" + i).SetValue(maximo, valorMaximo);
                        minimo.GetType().GetProperty("H" + i).SetValue(minimo, valorMinimo);

                        if (maxValue < valorMaximo) maxValue = valorMaximo;
                        if (minValue > valorMinimo) minValue = valorMinimo;
                        totalValue = totalValue + valorSuma;

                    }

                    suma.Meditotal = totalValue;
                    maximo.Meditotal = maxValue;
                    minimo.Meditotal = minValue;

                    if (tipoinfocodi == 1)
                    {
                        suma.Gruponomb = "TOTAL ENERGÍA (MWh)";
                        maximo.Gruponomb = "TOTAL POTENCIA MÁXIMA (MW)";
                        minimo.Gruponomb = "TOTAL POTENCIA MÍNIMA (MW)";
                    }

                    if (tipoinfocodi == 2)
                    {
                        suma.Gruponomb = "TOTAL ENERGÍA REACTIVA(MVarh)";
                        maximo.Gruponomb = "TOTAL POTENCIA REACTIVA MÁXIMA (MVAR)";
                        minimo.Gruponomb = "TOTAL POTENCIA REACTIVA MÍNIMA (MVAR)";
                    }

                    if (tipoptomedicodi == 56)
                    {
                        suma.Gruponomb = "TOTAL ENERGÍA REACTIVA CAPACITIVA (MVarh)";
                        maximo.Gruponomb = "TOTAL POTENCIA REACTIVA CAPACITIVA MÁXIMA (MVAR)";
                        minimo.Gruponomb = "TOTAL POTENCIA REACTIVA CAPACITIVA MÍNIMA (MVAR)";
                    }

                    if (tipoptomedicodi == 55)
                    {
                        suma.Gruponomb = "TOTAL ENERGÍA REACTIVA INDUCTIVA (MVarh)";
                        maximo.Gruponomb = "TOTAL POTENCIA REACTIVA INDUCTIVA MÁXIMA (MVAR)";
                        minimo.Gruponomb = "TOTAL POTENCIA REACTIVA INDUCTIVA MÍNIMA (MVAR)";
                    }

                    entitys.Add(suma);
                    entitys.Add(maximo);
                    entitys.Add(minimo);

                    #endregion
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerTotalServiciosAuxiliares(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string query = String.Format(helper.SqlTotalServiciosAuxiliares, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    #region Lectura

                    MeMedicion96DTO suma = new MeMedicion96DTO();
                    MeMedicion96DTO maximo = new MeMedicion96DTO();
                    MeMedicion96DTO minimo = new MeMedicion96DTO();

                    //int iMeditotal = dr.GetOrdinal(this.helper.Meditotal);
                    //if (!dr.IsDBNull(iMeditotal)) suma.Meditotal = dr.GetDecimal(iMeditotal) / 4;
                    //else suma.Meditotal = 0;
                    decimal totalValue = 0;
                    decimal maxValue = decimal.MinValue;
                    decimal minValue = decimal.MaxValue;


                    for (int i = 1; i <= 96; i++)
                    {
                        decimal valorSuma = 0;
                        decimal valorMaximo = 0;
                        decimal valorMinimo = 0;

                        int iSuma = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iSuma)) valorSuma = dr.GetDecimal(iSuma) / 4;

                        int iMaximo = dr.GetOrdinal("G" + i);
                        if (!dr.IsDBNull(iMaximo)) valorMaximo = dr.GetDecimal(iMaximo);

                        int iMinimo = dr.GetOrdinal("M" + i);
                        if (!dr.IsDBNull(iMinimo)) valorMinimo = dr.GetDecimal(iMinimo);

                        suma.GetType().GetProperty("H" + i).SetValue(suma, valorSuma);
                        maximo.GetType().GetProperty("H" + i).SetValue(maximo, valorMaximo);
                        minimo.GetType().GetProperty("H" + i).SetValue(minimo, valorMinimo);

                        if (maxValue < valorMaximo) maxValue = valorMaximo;
                        if (minValue > valorMinimo) minValue = valorMinimo;
                        totalValue = totalValue + valorSuma;
                    }

                    suma.Meditotal = totalValue;
                    maximo.Meditotal = maxValue;
                    minimo.Meditotal = minValue;

                    suma.Gruponomb = "TOTAL ENERGÍA SERV. AUX. (MWh)";
                    maximo.Gruponomb = "TOTAL POTENCIA MÁXIMA SERV. AUX. (MW)";
                    minimo.Gruponomb = "TOTAL POTENCIA MÍNIMA SERV. AUX. (MW)";

                    entitys.Add(suma);
                    entitys.Add(maximo);
                    entitys.Add(minimo);

                    #endregion
                }
            }

            return entitys;
        }

        public int ObtenerNroElementosConsultaMedidores(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi)
        {
            string query = String.Format(this.helper.SqlNroRegistrosConsultasTipoGeneracion, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int ObtenerNroElementosServiciosAuxiliares(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi)
        {
            string query = String.Format(this.helper.SqlNroRegistrosServiciosAuxiliares, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer, tipoptomedicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<MeMedicion96DTO> ListarTotalH(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string query = string.Format(helper.SqlListarTotalH, fechaini.ToString(ConstantesBase.FormatoFecha)
                , fechafin.ToString(ConstantesBase.FormatoFecha), empresas, tiposGeneracion, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ListarDetalle(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string query = string.Format(helper.SqlListarDetalle, fechaini.ToString(ConstantesBase.FormatoFecha)
               , fechafin.ToString(ConstantesBase.FormatoFecha), empresas, tiposGeneracion, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            MeMedicion96DTO entity;

            string Empresanomb = "EMPRESA";
            string Centralnomb = "CENTRAL";
            string Gruponomb = "GRUPO";
            string Tgenernomb = "TGENERNOMB";

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new MeMedicion96DTO();

                    entity = helper.Create(dr);
                    int iEmpresanomb = dr.GetOrdinal(Empresanomb);
                    if (!dr.IsDBNull(iEmpresanomb)) entity.Emprnomb = dr.GetString(iEmpresanomb);

                    int iCentralnomb = dr.GetOrdinal(Centralnomb);
                    if (!dr.IsDBNull(iCentralnomb)) entity.Central = dr.GetString(iCentralnomb);

                    int iGruponomb = dr.GetOrdinal(Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTgenernomb = dr.GetOrdinal(Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerReporteMedidores(string empresas, int tipoGrupoCodi, string fuenteEnergia,
            DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String query = String.Format(this.helper.SqlObtenerReporteMedidores, empresas, tipoGrupoCodi, fuenteEnergia,
                       fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = this.helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenergabrev = dr.GetOrdinal(this.helper.Fenergabrev);
                    if (!dr.IsDBNull(iFenergabrev)) entity.Fenergabrev = dr.GetString(iFenergabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerReporteMedidoresConsolidado(string empresas, int tipoGrupoCodi, string fuenteEnergia,
            DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String query = String.Format(this.helper.SqlObtenerReporteMedidoresConsolidado, empresas, tipoGrupoCodi, fuenteEnergia,
                       fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = this.helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenergabrev = dr.GetOrdinal(this.helper.Fenergabrev);
                    if (!dr.IsDBNull(iFenergabrev)) entity.Fenergabrev = dr.GetString(iFenergabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerDatosReporteMD(string empresas, int tipoGrupoCodi, string fuenteEnergia,
            DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String query = String.Format(this.helper.SqlObtenerDatosReporteMD, empresas, tipoGrupoCodi, fuenteEnergia,
                       fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(this.helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerMaximaDemandaPorRecursoEnergetico(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string query = string.Format(helper.SqlObtenerMaximaDemandaRecursoEnergetico, fechaini.ToString(ConstantesBase.FormatoFecha)
                , fechafin.ToString(ConstantesBase.FormatoFecha), empresas, tiposGeneracion, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerMaximaDemandaPorRecursoEnergeticoRER(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string query = string.Format(helper.SqlObtenerMaximaDemandaRecursoEnergeticoRER, fechaini.ToString(ConstantesBase.FormatoFecha)
                , fechafin.ToString(ConstantesBase.FormatoFecha), empresas, tiposGeneracion, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DateTime? ObtenerFechaMaximaDemanda(DateTime fechaInicio, DateTime fechaFin, string empresas, string tiposGeneracion,
            int central)
        {
            string query = string.Format(helper.SqlObtenerFechaMaximaDemanda, fechaInicio.ToString(ConstantesBase.FormatoFecha)
                    , fechaFin.ToString(ConstantesBase.FormatoFecha), empresas, tiposGeneracion, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object resultado = dbProvider.ExecuteScalar(command);

            if (resultado != null)
            {
                return Convert.ToDateTime(resultado);
            }

            return null;
        }

        public DateTime? ObtenerFechaMinimaDemanda(DateTime fechaInicio, DateTime fechaFin, string empresas, string tiposGeneracion,
            int central)
        {
            string query = string.Format(helper.SqlObtenerFechaMinimaDemanda, fechaInicio.ToString(ConstantesBase.FormatoFecha)
                        , fechaFin.ToString(ConstantesBase.FormatoFecha), empresas, tiposGeneracion, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object resultado = dbProvider.ExecuteScalar(command);

            if (resultado != null)
            {
                return Convert.ToDateTime(resultado);
            }

            return null;
        }
                
        public List<MeMedicion96DTO> ObtenerEnvioPorEmpresa(int emprcodi, DateTime fechaPeriodo)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String query = String.Format(this.helper.SqlObtenerEnvioPorEmpresa, emprcodi, fechaPeriodo.Year, fechaPeriodo.Month);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(this.helper.Create(dr));
                }
            }

            return entitys;

        }

        public void DeleteEnvioPorEmpresa(int emprcodi, DateTime fechaPeriodo)
        {
            String query = String.Format(this.helper.SqlDeleteEnvioPorEmpresa, fechaPeriodo.Year, fechaPeriodo.Month, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<ConsolidadoEnvioDTO> ConsolidadoEnvioXEmpresa(int emprcodi, DateTime fechaPeriodo)
        {
            List<ConsolidadoEnvioDTO> entitys = new List<ConsolidadoEnvioDTO>();
            ConsolidadoEnvioDTO entity;
            String query = String.Format(this.helper.SqlConsolidadoEnvioEmpresa, emprcodi, fechaPeriodo.Year, fechaPeriodo.Month);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new ConsolidadoEnvioDTO();

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGrupSSAA = dr.GetOrdinal(helper.GrupSSAA);
                    if (!dr.IsDBNull(iGrupSSAA)) entity.GrupSSAA = dr.GetString(iGrupSSAA);

                    int iTotal = dr.GetOrdinal(helper.Total);
                    if (!dr.IsDBNull(iTotal)) entity.Total = Convert.ToDecimal(dr.GetValue(iTotal));

                    int itipoGeneracion = dr.GetOrdinal(helper.Tipogeneracion);
                    if (!dr.IsDBNull(itipoGeneracion)) entity.TipoGeneracion = Convert.ToInt16(dr.GetValue(itipoGeneracion));

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        //inicio agregado
        #region Modificacion PR15 - 24/11/2017
        public List<ConsolidadoEnvioDTO> ConsolidadoEnvioByEmpresaAndFormato(int emprcodi, int lectcodi, int tipoinfocodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<ConsolidadoEnvioDTO> entitys = new List<ConsolidadoEnvioDTO>();
            ConsolidadoEnvioDTO entity;
            String query = String.Format(this.helper.SqlConsolidadoEnvioByEmpresaAndFormato, emprcodi, lectcodi, tipoinfocodi
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new ConsolidadoEnvioDTO();

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGrupSSAA = dr.GetOrdinal(helper.GrupSSAA);
                    if (!dr.IsDBNull(iGrupSSAA)) entity.GrupSSAA = dr.GetString(iGrupSSAA);

                    int iTotal = dr.GetOrdinal(helper.Total);
                    if (!dr.IsDBNull(iTotal)) entity.Total = Convert.ToDecimal(dr.GetValue(iTotal));

                    int itipoGeneracion = dr.GetOrdinal(helper.Tipogeneracion);
                    if (!dr.IsDBNull(itipoGeneracion)) entity.TipoGeneracion = Convert.ToInt16(dr.GetValue(itipoGeneracion));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
        //fin agregado

        public List<MeMedicion96DTO> ObtenerEnvioInterconexion(int emprcodi, DateTime fechaini, DateTime fechafin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            MeMedicion96DTO entity;
            string sqlQuery = string.Format(helper.SqlObtenerEnvioInterconexion, emprcodi, fechaini.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = this.helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerHistoricoInterconexion(int reporcodi, DateTime fechaini, DateTime fechafin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            MeMedicion96DTO entity;
            string sqlQuery = string.Format(helper.SqlObtenerHistoricoInterconexion, reporcodi, fechaini.ToString(ConstantesBase.FormatoFecha),
                fechafin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = this.helper.Create(dr);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int ObtenerTotalHistoricoInterconexion(int reporcodi, DateTime fechaini, DateTime fechafin)
        {
            string sqlTotal = string.Format(helper.SqlTotalHistoricoInterconexion, reporcodi,
                fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }

        public List<MeMedicion96DTO> ObtenerHistoricoPagInterconexion(int reporcodi, DateTime fechaini, DateTime fechafin, int pagina)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            MeMedicion96DTO entity;
            string sqlQuery = string.Format(helper.SqlHistoricoPagInterconexion, reporcodi, fechaini.ToString(ConstantesBase.FormatoFecha),
                fechafin.ToString(ConstantesBase.FormatoFecha), pagina);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = this.helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion96DTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivo, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> GetEnvioArchivo(int idFormato, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            string queryString = string.Format(helper.SqlGetEnvioArchivoSinHojaPuntoMed, idFormato, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> GetByCriteria(int idTipoInformacion, int idPtoMedicion, int idLectura, DateTime fechaInicio,
            DateTime fechaFin)
        {
            List<MeMedicion96DTO> entities = new List<MeMedicion96DTO>();
            string queryString = string.Format(helper.SqlGetByCriteria, idTipoInformacion, idPtoMedicion, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }
            return entities;
        }

        public void DeleteEnvioInterconexion(DateTime medifecha)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioInterconexion, medifecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa, DateTime fechaTiee)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo3, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa, fechaTiee.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        //inicio agregado
        public void DeleteEnvioArchivo2(int idTptomedi, int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa)
        {
            string sqlDelete = string.Format(helper.SqlDeleteEnvioArchivo2, idLectura, fechaInicio.ToString(ConstantesBase.FormatoFecha),
              fechaFin.ToString(ConstantesBase.FormatoFecha), idFormato, idEmpresa, idTptomedi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }
        //fin agregado

        public int ObtenerNroRegistrosMedDistribucion(string empresas, DateTime fechaInicio, DateTime fechaFin)
        {
            String query = String.Format(this.helper.SqlObtenerNroRegistrosMedDistribucion, empresas,
                       fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<MeMedicion96DTO> ObtenerConsultaMedDistribucion(string empresas, DateTime fechaInicio, DateTime fechaFin,
            int nroPagina, int nroRegistros)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String sql = String.Format(this.helper.SqlObtenerConsultaMedDistribucion, empresas,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPagina, nroRegistros);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquitension = dr.GetOrdinal(this.helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaoperativa = dr.GetOrdinal(this.helper.Areaoperativa);
                    if (!dr.IsDBNull(iAreaoperativa)) entity.Areaoperativa = dr.GetString(iAreaoperativa);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerTotalConsultaMedDistribucion(string empresas, DateTime fecInicio, DateTime fecFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            String query = String.Format(this.helper.SqlTotalConsultaMesDistribucion, empresas,
                       fecInicio.ToString(ConstantesBase.FormatoFecha), fecFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    MeMedicion96DTO suma = new MeMedicion96DTO();
                    decimal totalValue = 0;

                    for (int i = 1; i <= 96; i++)
                    {
                        decimal valorSuma = 0;
                        int iSuma = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iSuma)) valorSuma = dr.GetDecimal(iSuma);
                        suma.GetType().GetProperty("H" + i).SetValue(suma, valorSuma);
                        totalValue = totalValue + valorSuma;
                    }

                    suma.Meditotal = totalValue;
                    entitys.Add(suma);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerExportacionConsultaMedDistribucion(string empresas, DateTime fecInicio, DateTime fecFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String sql = String.Format(this.helper.SqlObtenerExportacionConsultaMedDistribucion, empresas,
                fecInicio.ToString(ConstantesBase.FormatoFecha), fecFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquitension = dr.GetOrdinal(this.helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreaoperativa = dr.GetOrdinal(this.helper.Areaoperativa);
                    if (!dr.IsDBNull(iAreaoperativa)) entity.Areaoperativa = dr.GetString(iAreaoperativa);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Update(MeMedicion96DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMedicion96DTO GetById(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            MeMedicion96DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedicion96DTO> GetByPtoMedicion(int ptomedicodi)
        {
            var entitys = new List<MeMedicion96DTO>();
            string queryString = string.Format(helper.SqlGetByPtoMedicion, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerConsultaWeb(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            string query = String.Format(helper.SqlObtenerConsultaWeb, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion48DTO> ObtenerConsultaWebReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = String.Format(helper.SqlObtenerConsultaWebReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTegenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTegenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTegenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);


                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Delete(int idPtomedicion, int idTipoInfo, DateTime fecha, int idLectura)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, idPtomedicion);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, idTipoInfo);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, idLectura);

            dbProvider.ExecuteNonQuery(command);

        }

        public List<DateTime> PaginacionInterconexiones(int idReporte, DateTime fechaInicio, DateTime fechaFin)
        {
            List<DateTime> entitys = new List<DateTime>();
            DateTime fecha;
            string strQuery = string.Format(helper.SqlPaginacionInterconexHistorico, idReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iFecha = dr.GetOrdinal("fecha");
                    if (!dr.IsDBNull(iFecha))
                    {
                        fecha = dr.GetDateTime(iFecha);
                        entitys.Add(fecha);
                    }
                }
            }
            return entitys;
        }

        public List<MeMedicion96DTO> SqlObtenerDatosEjecutado(DateTime fecha)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerDatosEjecutado, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();
                    entity = helper.Create(dr);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerListaObservacionCoherenciaMensual(int enviocodiact, int enviocodiant, string fecIniAct, string fecFinAct, string fecIniAnt,
            string fecFinAnt, int variacion)
        {
            string sqlQuery = string.Format(helper.SqlObtenerObservacionesCoherenciaMensual, fecIniAct, fecFinAct, fecIniAnt, fecFinAnt, enviocodiact, enviocodiant, variacion);
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();

                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoMediBarraNomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtoMediBarraNomb)) entity.PtoMediBarraNomb = dr.GetString(iPtoMediBarraNomb);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEnvioFechaPeriodoAct = dr.GetOrdinal(helper.Enviofechaperiodoact);
                    if (!dr.IsDBNull(iEnvioFechaPeriodoAct)) entity.EnvioFechaPeriodoAct = dr.GetDateTime(iEnvioFechaPeriodoAct);

                    int iEnvioFechaPeriodoAnt = dr.GetOrdinal(helper.Enviofechaperiodoant);
                    if (!dr.IsDBNull(iEnvioFechaPeriodoAnt)) entity.EnvioFechaPeriodoAnt = dr.GetDateTime(iEnvioFechaPeriodoAnt);

                    int iMeditotalAct = dr.GetOrdinal(helper.Meditotalact);
                    if (!dr.IsDBNull(iMeditotalAct)) entity.MeditotalAct = dr.GetDecimal(iMeditotalAct);

                    int iMeditotalAnt = dr.GetOrdinal(helper.Meditotalant);
                    if (!dr.IsDBNull(iMeditotalAnt)) entity.MeditotalAnt = dr.GetDecimal(iMeditotalAnt);

                    int iQAct = dr.GetOrdinal(helper.Qmedact);
                    if (!dr.IsDBNull(iQAct)) entity.QAct = Convert.ToInt32(dr.GetValue(iQAct));

                    int iQAnt = dr.GetOrdinal(helper.Qmedant);
                    if (!dr.IsDBNull(iQAnt)) entity.QAnt = Convert.ToInt32(dr.GetValue(iQAnt));

                    int iVarMensual = dr.GetOrdinal(helper.Varmensual);
                    if (!dr.IsDBNull(iVarMensual)) entity.VarMensual = dr.GetDecimal(iVarMensual);

                    int iVarPromDiaria = dr.GetOrdinal(helper.Varpromdiaria);
                    if (!dr.IsDBNull(iVarPromDiaria)) entity.VarPromDiaria = dr.GetDecimal(iVarPromDiaria);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerListaObservacionCoherenciaDiaria(int enviocodiact, int enviocodiant, string fecIniAct, string fecFinAct, string fecIniAnt,
            string fecFinAnt, int variacion)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerObservacionesCoherenciaDiaria, fecIniAct, fecFinAct, fecIniAnt, fecFinAnt, enviocodiact, enviocodiant, variacion);
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();

                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iPtoMediBarraNomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtoMediBarraNomb)) entity.PtoMediBarraNomb = dr.GetString(iPtoMediBarraNomb);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iEnvioFechaPeriodoAct = dr.GetOrdinal(helper.Enviofechaperiodoact);
                    if (!dr.IsDBNull(iEnvioFechaPeriodoAct)) entity.EnvioFechaPeriodoAct = dr.GetDateTime(iEnvioFechaPeriodoAct);

                    int iEnvioFechaPeriodoAnt = dr.GetOrdinal(helper.Enviofechaperiodoant);
                    if (!dr.IsDBNull(iEnvioFechaPeriodoAnt)) entity.EnvioFechaPeriodoAnt = dr.GetDateTime(iEnvioFechaPeriodoAnt);

                    int iMeditotalAct = dr.GetOrdinal(helper.Meditotalact);
                    if (!dr.IsDBNull(iMeditotalAct)) entity.MeditotalAct = dr.GetDecimal(iMeditotalAct);

                    int iMeditotalAnt = dr.GetOrdinal(helper.Meditotalant);
                    if (!dr.IsDBNull(iMeditotalAnt)) entity.MeditotalAnt = dr.GetDecimal(iMeditotalAnt);

                    int iQAct = dr.GetOrdinal(helper.Qmedact);
                    if (!dr.IsDBNull(iQAct)) entity.QAct = Convert.ToInt32(dr.GetValue(iQAct));

                    int iQAnt = dr.GetOrdinal(helper.Qmedant);
                    if (!dr.IsDBNull(iQAnt)) entity.QAnt = Convert.ToInt32(dr.GetValue(iQAnt));

                    int iVarMensual = dr.GetOrdinal(helper.Varmensual);
                    if (!dr.IsDBNull(iVarMensual)) entity.VarMensual = dr.GetDecimal(iVarMensual);

                    int iVarPromDiaria = dr.GetOrdinal(helper.Varpromdiaria);
                    if (!dr.IsDBNull(iVarPromDiaria)) entity.VarPromDiaria = dr.GetDecimal(iVarPromDiaria);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> GetListReporteInformacion15min(int formato, string fechaIni, string periodoSicli, string empresas, string tipos, string diasMaxDemanda, string lectCodiPR16, string lectCodiAlpha, int regIni, int regFin)
        {
            string qEmpresas = " 1 = 1";
            string qTipoEmpresas = "AND 1 = 1";
            string qMaxDemanda = " AND 1 = 1";
            string qpaginado = "";

            if (empresas != "" && empresas != null)
            {
                qEmpresas = " empr.EMPRCODI IN (" + empresas + ") ";
            }
            if (tipos != "")
            {
                qTipoEmpresas = " AND empr.TIPOEMPRCODI = " + tipos + " ";
            }
            if (diasMaxDemanda != "")
            {
                qMaxDemanda = " AND TO_CHAR(pees.FECHA_FILA, 'dd/mm/yyyy') IN (" + diasMaxDemanda + ") ";
            }

            if (regIni != 0 && regFin != 0)
            {
                //qpaginado = "WHERE exte.item >= " + regIni + " AND exte.item <= " + regFin;
                //qpaginado = " where exte.meditotal > 0";
            }

            qpaginado = " where exte.meditotal > 0";
            //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
            //string sqlQuery = string.Format(this.helper.SqlListReporteInformacion
            //                              , formato
            //                              , fechaIni
            //                              , periodoSicli
            //                              , qEmpresas
            //                              , qTipoEmpresas
            //                              , qMaxDemanda
            //                              , lectCodiPR16
            //                              , lectCodiAlpha
            //                              , qpaginado);
            string sqlQuery = string.Format(this.helper.SqlListReporteInformacion
                                          , formato
                                          , fechaIni
                                          , periodoSicli
                                          , qEmpresas
                                          , qTipoEmpresas
                                          , qMaxDemanda
                                          , lectCodiPR16
                                          , lectCodiAlpha
                                          , qpaginado
                                          , tipos);
            //- HDT Fin

            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();

                    int iItem = dr.GetOrdinal(helper.Item);
                    if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                    int iFuente = dr.GetOrdinal(helper.Fuente);
                    if (!dr.IsDBNull(iFuente)) entity.Fuente = dr.GetString(iFuente);

                    int iFechaFila = dr.GetOrdinal(helper.FechaFila);
                    if (!dr.IsDBNull(iFechaFila)) entity.FechaFila = dr.GetDateTime(iFechaFila);

                    int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
                    if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

                    int iCodigoCliente = dr.GetOrdinal(helper.CodigoCliente);
                    if (!dr.IsDBNull(iCodigoCliente)) entity.CodigoCliente = dr.GetString(iCodigoCliente);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRucEmpresa = dr.GetOrdinal(helper.RucEmpresa);
                    if (!dr.IsDBNull(iRucEmpresa)) entity.RucEmpresa = dr.GetString(iRucEmpresa);

                    int iNombreEmpresa = dr.GetOrdinal(helper.NombreEmpresa);
                    if (!dr.IsDBNull(iNombreEmpresa)) entity.NombreEmpresa = dr.GetString(iNombreEmpresa);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = dr.GetString(iTension);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    int iNroEnvios = dr.GetOrdinal(helper.NroEnvios);
                    if (!dr.IsDBNull(iNroEnvios)) entity.NroEnvios = Convert.ToInt32(dr.GetValue(iNroEnvios));

                    int iFechaPrimerEnvio = dr.GetOrdinal(helper.FechaPrimerEnvio);
                    if (!dr.IsDBNull(iFechaPrimerEnvio)) entity.FechaPrimerEnvio = dr.GetDateTime(iFechaPrimerEnvio);

                    int iFechaUltimoEnvio = dr.GetOrdinal(helper.FechaUltimoEnvio);
                    if (!dr.IsDBNull(iFechaUltimoEnvio)) entity.FechaUltimoEnvio = dr.GetDateTime(iFechaUltimoEnvio);

                    int iIniRemision = dr.GetOrdinal(helper.IniRemision);
                    if (!dr.IsDBNull(iIniRemision)) entity.IniRemision = dr.GetDateTime(iIniRemision);

                    int iFinRemision = dr.GetOrdinal(helper.FinRemision);
                    if (!dr.IsDBNull(iFinRemision)) entity.FinRemision = dr.GetDateTime(iFinRemision);

                    int iIniPeriodo = dr.GetOrdinal(helper.IniPeriodo);
                    if (!dr.IsDBNull(iIniPeriodo)) entity.IniPeriodo = dr.GetDateTime(iIniPeriodo);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int GetListReporteInformacion15minCount(int formato, string fechaIni, string periodoSicli, string empresas, string tipos, string diasMaxDemanda, string lectCodiPR16, string lectCodiAlpha)
        {
            string qEmpresas = " 1 = 1";
            string qTipoEmpresas = "AND 1 = 1";
            string qMaxDemanda = " AND 1 = 1";

            if (empresas != "" && empresas != null)
            {
                qEmpresas = " empr.EMPRCODI IN (" + empresas + ") ";
            }
            if (tipos != "")
            {
                qTipoEmpresas = " AND empr.TIPOEMPRCODI = " + tipos + " ";
            }
            if (diasMaxDemanda != "")
            {
                qMaxDemanda = " AND TO_CHAR(pees.FECHA_FILA, 'dd/mm/yyyy') IN (" + diasMaxDemanda + ") ";
            }

            //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
            //string sqlQuery = string.Format(this.helper.SqlListReporteInformacionCount, formato, fechaIni, fechaFin, qEmpresas, qTipoEmpresas, qMaxDemanda, lectCodiPR16, lectCodiAlpha);
            string sqlQuery = string.Format(this.helper.SqlListReporteInformacionCount
                                          , formato
                                          , fechaIni
                                          , periodoSicli
                                          , qEmpresas
                                          , qTipoEmpresas
                                          , qMaxDemanda
                                          , lectCodiPR16
                                          , lectCodiAlpha
                                          , tipos);
            //- HDT Fin
            
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal(helper.Qregistros);
                    cant = Convert.ToInt32(dr.GetValue(iQregistros));
                }
            }
            return cant;
        }

        public void Delete(int idPtomedicion, int idTipoInfo, DateTime fechaInicio, DateTime fechaFin, int idLectura)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteFechas);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, idPtomedicion);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, idTipoInfo);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fechaInicio);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, idLectura);

            dbProvider.ExecuteNonQuery(command);

        }
        //inicio modificado
        public List<MeMedicion96DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, int ptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string sql = String.Format(helper.SqlObtenerMedicion96, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), lectcodi, tipoinfocodi, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTipoptomedicodi = dr.GetOrdinal(helper.Tipoptomedicodi);
                    if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //fin modificado

        //inicio agregado
        public List<MeMedicion96DTO> GetResumenMaximaDemanda(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string sql = String.Format(helper.SqlGetResumenMaximaDemanda, tipoCentral, tipoGeneracion, famcodiSSAA, tipogrupocodiNoIntegrante
                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi, tipogrupocodiRer);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();
                    int iMediFecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMediFecha)) entity.Medifecha = dr.GetDateTime(iMediFecha);

                    this.helper.GetH1To96(dr, entity);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion96DTO> GetConsolidadoMaximaDemanda(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            String query = String.Format(this.helper.SqlGetConsolidadoMaximaDemanda, tipoCentral, tipoGeneracion
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idEmpresa, lectcodi, tipoinfocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();
                    entity = helper.Create(dr);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iTipogenerrer = dr.GetOrdinal(helper.Tipogenerrer);
                    if (!dr.IsDBNull(iTipogenerrer)) entity.Tipogenerrer = dr.GetString(iTipogenerrer);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    //Ticket-6068
                    int iOsinergcodiGen = dr.GetOrdinal(this.helper.OsinergcodiGen);
                    if (!dr.IsDBNull(iOsinergcodiGen)) entity.OsinergcodiGen = dr.GetString(iOsinergcodiGen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MeMedicion96DTO> GetConsolidadoExcesoPotenReact(int tipoCentral, int tipoGeneracion, string idEmpresa, string fechaIni, string fechaFin, int famcodiSSAA, int lectcodi, int tipoinfocodi, int tptomedicodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            String query = String.Format(this.helper.SqlGetConsolidadoExcesoPotenReact, tipoCentral, tipoGeneracion, Convert.ToInt32(idEmpresa), fechaIni, fechaFin, famcodiSSAA, lectcodi, tipoinfocodi, tptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();

                    int iNOMBGRUPO = dr.GetOrdinal("NOMBGRUPO");
                    if (!dr.IsDBNull(iNOMBGRUPO)) entity.Gruponomb = dr.GetString(iNOMBGRUPO);

                    int iHOPCODI = dr.GetOrdinal("HOPCODI");
                    if (!dr.IsDBNull(iHOPCODI)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHOPCODI));

                    int iHOPHORINI = dr.GetOrdinal("HOPHORINI");
                    if (!dr.IsDBNull(iHOPHORINI)) entity.Hophorini = dr.GetDateTime(iHOPHORINI);

                    int iHOPHORFIN = dr.GetOrdinal("HOPHORFIN");
                    if (!dr.IsDBNull(iHOPHORFIN)) entity.Hophorfin = dr.GetDateTime(iHOPHORFIN);

                    int iSUBCAUSACODI = dr.GetOrdinal("SUBCAUSACODI");
                    if (!dr.IsDBNull(iSUBCAUSACODI)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSUBCAUSACODI));

                    int iTGENERNOMB = dr.GetOrdinal("TGENERNOMB");
                    if (!dr.IsDBNull(iTGENERNOMB)) entity.Tgenernomb = dr.GetString(iTGENERNOMB);

                    int iTGENERCODI = dr.GetOrdinal("TGENERCODI");
                    if (!dr.IsDBNull(iTGENERCODI)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTGENERCODI));

                    int iPTOMEDICODI = dr.GetOrdinal("PTOMEDICODI");
                    if (!dr.IsDBNull(iPTOMEDICODI)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPTOMEDICODI));

                    int iGRUPO = dr.GetOrdinal("GRUPO");
                    if (!dr.IsDBNull(iGRUPO)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGRUPO));

                    int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEMPRCODI));

                    int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.GetString(iEMPRNOMB);

                    int iEQUICODI = dr.GetOrdinal("EQUICODI");
                    if (!dr.IsDBNull(iEQUICODI)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEQUICODI));

                    int iEQUINOMB = dr.GetOrdinal("EQUINOMB");
                    if (!dr.IsDBNull(iEQUINOMB)) entity.Equinomb = dr.GetString(iEQUINOMB);

                    int iPERIODO = dr.GetOrdinal("PERIODO");
                    if (!dr.IsDBNull(iPERIODO)) entity.DatePeriodo = dr.GetDateTime(iPERIODO);

                    int iLECTCODI = dr.GetOrdinal("LECTCODI");
                    if (!dr.IsDBNull(iLECTCODI)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLECTCODI));

                    int iTIPOINFOCODI = dr.GetOrdinal("TIPOINFOCODI");
                    if (!dr.IsDBNull(iTIPOINFOCODI)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTIPOINFOCODI));

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    int iTPTOMEDICODI = dr.GetOrdinal("TPTOMEDICODI");
                    if (!dr.IsDBNull(iTPTOMEDICODI)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTPTOMEDICODI));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //fin agregado

        //metodo grabacion para demanda Mercado Libre

        public void SaveDemandaMercadoLibre(MeMedicion96DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveDemandaMercadoLibre);

            dbProvider.AddInParameter(command, helper.H96, DbType.Decimal, entity.H96);
            dbProvider.AddInParameter(command, helper.H95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.H94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.H92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.H90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.H88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.H86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.H84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.H82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.H80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.H78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.H76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.H74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.H72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.H70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.H68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.H66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.H64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.H62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Mediestado, DbType.String, entity.Mediestado);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.TptoMediCodi, DbType.Int32, entity.TptoMediCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        #region FIT - Aplicativo VTP

        public List<MeMedicion96DTO> GetListFullInformacionPrevistaRPorParticipante(DateTime fechaInicio, DateTime fechaFinal, int emprcodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string sCommand = string.Format(helper.SqlListByFilter, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);
            //MeMedicion96DTO entity = new MeMedicion96DTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();
                    //entity = helper.Create(dr);

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedibarranomb = dr.GetOrdinal(this.helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.PtoMediBarraNomb = dr.GetString(iPtomedibarranomb);

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(this.helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(this.helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(this.helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(this.helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(this.helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(this.helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(this.helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(this.helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(this.helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(this.helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(this.helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(this.helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(this.helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(this.helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(this.helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(this.helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(this.helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(this.helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(this.helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(this.helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(this.helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(this.helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(this.helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(this.helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(this.helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(this.helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(this.helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(this.helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(this.helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(this.helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(this.helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(this.helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(this.helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(this.helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(this.helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(this.helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(this.helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    int iclienteNomb = dr.GetOrdinal(helper.ClienteNomb);
                    if (!dr.IsDBNull(iclienteNomb)) entity.ClienteNomb = dr.GetString(iclienteNomb);

                    int ibarrNomb = dr.GetOrdinal(helper.BarrNomb);
                    if (!dr.IsDBNull(ibarrNomb)) entity.BarrNomb = dr.GetString(ibarrNomb);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion96DTO> GetListPageByFilterInformacionPrevistaRPorParticipante(DateTime fechaInicio, DateTime fechaFinal, int emprcodi, int nroPage, int pageSize)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();

            string sCommand = string.Format(helper.SqlListPagedByFilter, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), emprcodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();
                    //entity = helper.Create(dr);

                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedibarranomb = dr.GetOrdinal(this.helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.PtoMediBarraNomb = dr.GetString(iPtomedibarranomb);

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(this.helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(this.helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(this.helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(this.helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(this.helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(this.helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(this.helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(this.helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(this.helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(this.helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(this.helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(this.helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(this.helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(this.helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(this.helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(this.helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(this.helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(this.helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(this.helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(this.helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(this.helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(this.helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(this.helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(this.helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(this.helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(this.helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(this.helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(this.helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(this.helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(this.helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(this.helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(this.helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(this.helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(this.helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(this.helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(this.helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(this.helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);

                    int iclienteNomb = dr.GetOrdinal(helper.ClienteNomb);
                    if (!dr.IsDBNull(iclienteNomb)) entity.ClienteNomb = dr.GetString(iclienteNomb);

                    int ibarrNomb = dr.GetOrdinal(helper.BarrNomb);
                    if (!dr.IsDBNull(ibarrNomb)) entity.BarrNomb = dr.GetString(ibarrNomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        public List<MeMedicion96DTO> ListaNumerales_DatosBase_5_1_1(string fechaini, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_1_1, fechaini, fechaFin);

            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);


                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));



                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeMedicion96DTO> ObtenerDatosMedicionComparativo(DateTime fechaInicio, DateTime fechaFin, string puntos,
            int lectcodi, int tipoInfoCodi)
        {
            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            string query = string.Format(helper.SqlObtenerDatosMedicionComparativo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), puntos,
                lectcodi, tipoInfoCodi);        
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion96DTO entity = new MeMedicion96DTO();
                    this.helper.GetH1To96(dr, entity);
                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}


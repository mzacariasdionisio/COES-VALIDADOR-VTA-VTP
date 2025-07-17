using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_GENERDEMAN
    /// </summary>
    public class CaiGenerdemanRepository: RepositoryBase, ICaiGenerdemanRepository
    {
        public CaiGenerdemanRepository(string strConn): base(strConn)
        {
        }

        CaiGenerdemanHelper helper = new CaiGenerdemanHelper();

        public Int32 GetCodigoGenerado()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            Int32 id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public Int32 Save(CaiGenerdemanDTO entity)
        {
            Int32 id = GetCodigoGenerado();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cagdcmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Cagdcmfuentedat, DbType.String, entity.Cagdcmfuentedat);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Cagdcmcalidadinfo, DbType.String, entity.Cagdcmcalidadinfo);
            dbProvider.AddInParameter(command, helper.Cagdcmdia, DbType.DateTime, entity.Cagdcmdia);
            dbProvider.AddInParameter(command, helper.Cagdcmtotaldia, DbType.Decimal, entity.Cagdcmtotaldia);
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
            dbProvider.AddInParameter(command, helper.T1, DbType.String, entity.T1);
            dbProvider.AddInParameter(command, helper.T2, DbType.String, entity.T2);
            dbProvider.AddInParameter(command, helper.T3, DbType.String, entity.T3);
            dbProvider.AddInParameter(command, helper.T4, DbType.String, entity.T4);
            dbProvider.AddInParameter(command, helper.T5, DbType.String, entity.T5);
            dbProvider.AddInParameter(command, helper.T6, DbType.String, entity.T6);
            dbProvider.AddInParameter(command, helper.T7, DbType.String, entity.T7);
            dbProvider.AddInParameter(command, helper.T8, DbType.String, entity.T8);
            dbProvider.AddInParameter(command, helper.T9, DbType.String, entity.T9);
            dbProvider.AddInParameter(command, helper.T10, DbType.String, entity.T10);
            dbProvider.AddInParameter(command, helper.T11, DbType.String, entity.T11);
            dbProvider.AddInParameter(command, helper.T12, DbType.String, entity.T12);
            dbProvider.AddInParameter(command, helper.T13, DbType.String, entity.T13);
            dbProvider.AddInParameter(command, helper.T14, DbType.String, entity.T14);
            dbProvider.AddInParameter(command, helper.T15, DbType.String, entity.T15);
            dbProvider.AddInParameter(command, helper.T16, DbType.String, entity.T16);
            dbProvider.AddInParameter(command, helper.T17, DbType.String, entity.T17);
            dbProvider.AddInParameter(command, helper.T18, DbType.String, entity.T18);
            dbProvider.AddInParameter(command, helper.T19, DbType.String, entity.T19);
            dbProvider.AddInParameter(command, helper.T20, DbType.String, entity.T20);
            dbProvider.AddInParameter(command, helper.T21, DbType.String, entity.T21);
            dbProvider.AddInParameter(command, helper.T22, DbType.String, entity.T22);
            dbProvider.AddInParameter(command, helper.T23, DbType.String, entity.T23);
            dbProvider.AddInParameter(command, helper.T24, DbType.String, entity.T24);
            dbProvider.AddInParameter(command, helper.T25, DbType.String, entity.T25);
            dbProvider.AddInParameter(command, helper.T26, DbType.String, entity.T26);
            dbProvider.AddInParameter(command, helper.T27, DbType.String, entity.T27);
            dbProvider.AddInParameter(command, helper.T28, DbType.String, entity.T28);
            dbProvider.AddInParameter(command, helper.T29, DbType.String, entity.T29);
            dbProvider.AddInParameter(command, helper.T30, DbType.String, entity.T30);
            dbProvider.AddInParameter(command, helper.T31, DbType.String, entity.T31);
            dbProvider.AddInParameter(command, helper.T32, DbType.String, entity.T32);
            dbProvider.AddInParameter(command, helper.T33, DbType.String, entity.T33);
            dbProvider.AddInParameter(command, helper.T34, DbType.String, entity.T34);
            dbProvider.AddInParameter(command, helper.T35, DbType.String, entity.T35);
            dbProvider.AddInParameter(command, helper.T36, DbType.String, entity.T36);
            dbProvider.AddInParameter(command, helper.T37, DbType.String, entity.T37);
            dbProvider.AddInParameter(command, helper.T38, DbType.String, entity.T38);
            dbProvider.AddInParameter(command, helper.T39, DbType.String, entity.T39);
            dbProvider.AddInParameter(command, helper.T40, DbType.String, entity.T40);
            dbProvider.AddInParameter(command, helper.T41, DbType.String, entity.T41);
            dbProvider.AddInParameter(command, helper.T42, DbType.String, entity.T42);
            dbProvider.AddInParameter(command, helper.T43, DbType.String, entity.T43);
            dbProvider.AddInParameter(command, helper.T44, DbType.String, entity.T44);
            dbProvider.AddInParameter(command, helper.T45, DbType.String, entity.T45);
            dbProvider.AddInParameter(command, helper.T46, DbType.String, entity.T46);
            dbProvider.AddInParameter(command, helper.T47, DbType.String, entity.T47);
            dbProvider.AddInParameter(command, helper.T48, DbType.String, entity.T48);
            dbProvider.AddInParameter(command, helper.T49, DbType.String, entity.T49);
            dbProvider.AddInParameter(command, helper.T50, DbType.String, entity.T50);
            dbProvider.AddInParameter(command, helper.T51, DbType.String, entity.T51);
            dbProvider.AddInParameter(command, helper.T52, DbType.String, entity.T52);
            dbProvider.AddInParameter(command, helper.T53, DbType.String, entity.T53);
            dbProvider.AddInParameter(command, helper.T54, DbType.String, entity.T54);
            dbProvider.AddInParameter(command, helper.T55, DbType.String, entity.T55);
            dbProvider.AddInParameter(command, helper.T56, DbType.String, entity.T56);
            dbProvider.AddInParameter(command, helper.T57, DbType.String, entity.T57);
            dbProvider.AddInParameter(command, helper.T58, DbType.String, entity.T58);
            dbProvider.AddInParameter(command, helper.T59, DbType.String, entity.T59);
            dbProvider.AddInParameter(command, helper.T60, DbType.String, entity.T60);
            dbProvider.AddInParameter(command, helper.T61, DbType.String, entity.T61);
            dbProvider.AddInParameter(command, helper.T62, DbType.String, entity.T62);
            dbProvider.AddInParameter(command, helper.T63, DbType.String, entity.T63);
            dbProvider.AddInParameter(command, helper.T64, DbType.String, entity.T64);
            dbProvider.AddInParameter(command, helper.T65, DbType.String, entity.T65);
            dbProvider.AddInParameter(command, helper.T66, DbType.String, entity.T66);
            dbProvider.AddInParameter(command, helper.T67, DbType.String, entity.T67);
            dbProvider.AddInParameter(command, helper.T68, DbType.String, entity.T68);
            dbProvider.AddInParameter(command, helper.T69, DbType.String, entity.T69);
            dbProvider.AddInParameter(command, helper.T70, DbType.String, entity.T70);
            dbProvider.AddInParameter(command, helper.T71, DbType.String, entity.T71);
            dbProvider.AddInParameter(command, helper.T72, DbType.String, entity.T72);
            dbProvider.AddInParameter(command, helper.T73, DbType.String, entity.T73);
            dbProvider.AddInParameter(command, helper.T74, DbType.String, entity.T74);
            dbProvider.AddInParameter(command, helper.T75, DbType.String, entity.T75);
            dbProvider.AddInParameter(command, helper.T76, DbType.String, entity.T76);
            dbProvider.AddInParameter(command, helper.T77, DbType.String, entity.T77);
            dbProvider.AddInParameter(command, helper.T78, DbType.String, entity.T78);
            dbProvider.AddInParameter(command, helper.T79, DbType.String, entity.T79);
            dbProvider.AddInParameter(command, helper.T80, DbType.String, entity.T80);
            dbProvider.AddInParameter(command, helper.T81, DbType.String, entity.T81);
            dbProvider.AddInParameter(command, helper.T82, DbType.String, entity.T82);
            dbProvider.AddInParameter(command, helper.T83, DbType.String, entity.T83);
            dbProvider.AddInParameter(command, helper.T84, DbType.String, entity.T84);
            dbProvider.AddInParameter(command, helper.T85, DbType.String, entity.T85);
            dbProvider.AddInParameter(command, helper.T86, DbType.String, entity.T86);
            dbProvider.AddInParameter(command, helper.T87, DbType.String, entity.T87);
            dbProvider.AddInParameter(command, helper.T88, DbType.String, entity.T88);
            dbProvider.AddInParameter(command, helper.T89, DbType.String, entity.T89);
            dbProvider.AddInParameter(command, helper.T90, DbType.String, entity.T90);
            dbProvider.AddInParameter(command, helper.T91, DbType.String, entity.T91);
            dbProvider.AddInParameter(command, helper.T92, DbType.String, entity.T92);
            dbProvider.AddInParameter(command, helper.T93, DbType.String, entity.T93);
            dbProvider.AddInParameter(command, helper.T94, DbType.String, entity.T94);
            dbProvider.AddInParameter(command, helper.T95, DbType.String, entity.T95);
            dbProvider.AddInParameter(command, helper.T96, DbType.String, entity.T96);
            dbProvider.AddInParameter(command, helper.Cagdcmusucreacion, DbType.String, entity.Cagdcmusucreacion);
            dbProvider.AddInParameter(command, helper.Cagdcmfeccreacion, DbType.DateTime, entity.Cagdcmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void SaveAsSelectUsuariosLibres(Int32 cagdcmcodi, int caiajcodi, string cagdcmfuentedat, string cagdcmcalidadinfo, string T, string user, int Formatcodi, string FechaInicio, string FechaFin, int TipoEmprcodi, string lectCodiPR16, string lectCodiAlpha)
        {
            string sqlQuery = string.Format(this.helper.SqlSaveAsSelectUsuariosLibres, cagdcmcodi, caiajcodi, cagdcmfuentedat, cagdcmcalidadinfo, T, user, Formatcodi, FechaInicio, FechaFin, TipoEmprcodi, lectCodiPR16, lectCodiAlpha);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveCaiGenerdemanAsSelectMeMedicion96(int cagdcmcodi, int caiajcodi, string cagdcmfuentedat, string cagdcmcalidadinfo, string tipodato, string user, int Formatcodi, int Lectcodi, string FechaInicio, string FechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlSaveAsSelectMeMedicion96, cagdcmcodi, caiajcodi, cagdcmfuentedat, cagdcmcalidadinfo, tipodato, user, Formatcodi, Lectcodi, FechaInicio, FechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CaiGenerdemanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Cagdcmfuentedat, DbType.String, entity.Cagdcmfuentedat);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Cagdcmcalidadinfo, DbType.String, entity.Cagdcmcalidadinfo);
            dbProvider.AddInParameter(command, helper.Cagdcmdia, DbType.DateTime, entity.Cagdcmdia);
            dbProvider.AddInParameter(command, helper.Cagdcmtotaldia, DbType.Decimal, entity.Cagdcmtotaldia);
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
            dbProvider.AddInParameter(command, helper.T1, DbType.String, entity.T1);
            dbProvider.AddInParameter(command, helper.T2, DbType.String, entity.T2);
            dbProvider.AddInParameter(command, helper.T3, DbType.String, entity.T3);
            dbProvider.AddInParameter(command, helper.T4, DbType.String, entity.T4);
            dbProvider.AddInParameter(command, helper.T5, DbType.String, entity.T5);
            dbProvider.AddInParameter(command, helper.T6, DbType.String, entity.T6);
            dbProvider.AddInParameter(command, helper.T7, DbType.String, entity.T7);
            dbProvider.AddInParameter(command, helper.T8, DbType.String, entity.T8);
            dbProvider.AddInParameter(command, helper.T9, DbType.String, entity.T9);
            dbProvider.AddInParameter(command, helper.T10, DbType.String, entity.T10);
            dbProvider.AddInParameter(command, helper.T11, DbType.String, entity.T11);
            dbProvider.AddInParameter(command, helper.T12, DbType.String, entity.T12);
            dbProvider.AddInParameter(command, helper.T13, DbType.String, entity.T13);
            dbProvider.AddInParameter(command, helper.T14, DbType.String, entity.T14);
            dbProvider.AddInParameter(command, helper.T15, DbType.String, entity.T15);
            dbProvider.AddInParameter(command, helper.T16, DbType.String, entity.T16);
            dbProvider.AddInParameter(command, helper.T17, DbType.String, entity.T17);
            dbProvider.AddInParameter(command, helper.T18, DbType.String, entity.T18);
            dbProvider.AddInParameter(command, helper.T19, DbType.String, entity.T19);
            dbProvider.AddInParameter(command, helper.T20, DbType.String, entity.T20);
            dbProvider.AddInParameter(command, helper.T21, DbType.String, entity.T21);
            dbProvider.AddInParameter(command, helper.T22, DbType.String, entity.T22);
            dbProvider.AddInParameter(command, helper.T23, DbType.String, entity.T23);
            dbProvider.AddInParameter(command, helper.T24, DbType.String, entity.T24);
            dbProvider.AddInParameter(command, helper.T25, DbType.String, entity.T25);
            dbProvider.AddInParameter(command, helper.T26, DbType.String, entity.T26);
            dbProvider.AddInParameter(command, helper.T27, DbType.String, entity.T27);
            dbProvider.AddInParameter(command, helper.T28, DbType.String, entity.T28);
            dbProvider.AddInParameter(command, helper.T29, DbType.String, entity.T29);
            dbProvider.AddInParameter(command, helper.T30, DbType.String, entity.T30);
            dbProvider.AddInParameter(command, helper.T31, DbType.String, entity.T31);
            dbProvider.AddInParameter(command, helper.T32, DbType.String, entity.T32);
            dbProvider.AddInParameter(command, helper.T33, DbType.String, entity.T33);
            dbProvider.AddInParameter(command, helper.T34, DbType.String, entity.T34);
            dbProvider.AddInParameter(command, helper.T35, DbType.String, entity.T35);
            dbProvider.AddInParameter(command, helper.T36, DbType.String, entity.T36);
            dbProvider.AddInParameter(command, helper.T37, DbType.String, entity.T37);
            dbProvider.AddInParameter(command, helper.T38, DbType.String, entity.T38);
            dbProvider.AddInParameter(command, helper.T39, DbType.String, entity.T39);
            dbProvider.AddInParameter(command, helper.T40, DbType.String, entity.T40);
            dbProvider.AddInParameter(command, helper.T41, DbType.String, entity.T41);
            dbProvider.AddInParameter(command, helper.T42, DbType.String, entity.T42);
            dbProvider.AddInParameter(command, helper.T43, DbType.String, entity.T43);
            dbProvider.AddInParameter(command, helper.T44, DbType.String, entity.T44);
            dbProvider.AddInParameter(command, helper.T45, DbType.String, entity.T45);
            dbProvider.AddInParameter(command, helper.T46, DbType.String, entity.T46);
            dbProvider.AddInParameter(command, helper.T47, DbType.String, entity.T47);
            dbProvider.AddInParameter(command, helper.T48, DbType.String, entity.T48);
            dbProvider.AddInParameter(command, helper.T49, DbType.String, entity.T49);
            dbProvider.AddInParameter(command, helper.T50, DbType.String, entity.T50);
            dbProvider.AddInParameter(command, helper.T51, DbType.String, entity.T51);
            dbProvider.AddInParameter(command, helper.T52, DbType.String, entity.T52);
            dbProvider.AddInParameter(command, helper.T53, DbType.String, entity.T53);
            dbProvider.AddInParameter(command, helper.T54, DbType.String, entity.T54);
            dbProvider.AddInParameter(command, helper.T55, DbType.String, entity.T55);
            dbProvider.AddInParameter(command, helper.T56, DbType.String, entity.T56);
            dbProvider.AddInParameter(command, helper.T57, DbType.String, entity.T57);
            dbProvider.AddInParameter(command, helper.T58, DbType.String, entity.T58);
            dbProvider.AddInParameter(command, helper.T59, DbType.String, entity.T59);
            dbProvider.AddInParameter(command, helper.T60, DbType.String, entity.T60);
            dbProvider.AddInParameter(command, helper.T61, DbType.String, entity.T61);
            dbProvider.AddInParameter(command, helper.T62, DbType.String, entity.T62);
            dbProvider.AddInParameter(command, helper.T63, DbType.String, entity.T63);
            dbProvider.AddInParameter(command, helper.T64, DbType.String, entity.T64);
            dbProvider.AddInParameter(command, helper.T65, DbType.String, entity.T65);
            dbProvider.AddInParameter(command, helper.T66, DbType.String, entity.T66);
            dbProvider.AddInParameter(command, helper.T67, DbType.String, entity.T67);
            dbProvider.AddInParameter(command, helper.T68, DbType.String, entity.T68);
            dbProvider.AddInParameter(command, helper.T69, DbType.String, entity.T69);
            dbProvider.AddInParameter(command, helper.T70, DbType.String, entity.T70);
            dbProvider.AddInParameter(command, helper.T71, DbType.String, entity.T71);
            dbProvider.AddInParameter(command, helper.T72, DbType.String, entity.T72);
            dbProvider.AddInParameter(command, helper.T73, DbType.String, entity.T73);
            dbProvider.AddInParameter(command, helper.T74, DbType.String, entity.T74);
            dbProvider.AddInParameter(command, helper.T75, DbType.String, entity.T75);
            dbProvider.AddInParameter(command, helper.T76, DbType.String, entity.T76);
            dbProvider.AddInParameter(command, helper.T77, DbType.String, entity.T77);
            dbProvider.AddInParameter(command, helper.T78, DbType.String, entity.T78);
            dbProvider.AddInParameter(command, helper.T79, DbType.String, entity.T79);
            dbProvider.AddInParameter(command, helper.T80, DbType.String, entity.T80);
            dbProvider.AddInParameter(command, helper.T81, DbType.String, entity.T81);
            dbProvider.AddInParameter(command, helper.T82, DbType.String, entity.T82);
            dbProvider.AddInParameter(command, helper.T83, DbType.String, entity.T83);
            dbProvider.AddInParameter(command, helper.T84, DbType.String, entity.T84);
            dbProvider.AddInParameter(command, helper.T85, DbType.String, entity.T85);
            dbProvider.AddInParameter(command, helper.T86, DbType.String, entity.T86);
            dbProvider.AddInParameter(command, helper.T87, DbType.String, entity.T87);
            dbProvider.AddInParameter(command, helper.T88, DbType.String, entity.T88);
            dbProvider.AddInParameter(command, helper.T89, DbType.String, entity.T89);
            dbProvider.AddInParameter(command, helper.T90, DbType.String, entity.T90);
            dbProvider.AddInParameter(command, helper.T91, DbType.String, entity.T91);
            dbProvider.AddInParameter(command, helper.T92, DbType.String, entity.T92);
            dbProvider.AddInParameter(command, helper.T93, DbType.String, entity.T93);
            dbProvider.AddInParameter(command, helper.T94, DbType.String, entity.T94);
            dbProvider.AddInParameter(command, helper.T95, DbType.String, entity.T95);
            dbProvider.AddInParameter(command, helper.T96, DbType.String, entity.T96);
            dbProvider.AddInParameter(command, helper.Cagdcmusucreacion, DbType.String, entity.Cagdcmusucreacion);
            dbProvider.AddInParameter(command, helper.Cagdcmfeccreacion, DbType.DateTime, entity.Cagdcmfeccreacion);
            dbProvider.AddInParameter(command, helper.Cagdcmcodi, DbType.Int32, entity.Cagdcmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajcodi, string cagdcmfuentedat)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            dbProvider.AddInParameter(command, helper.Cagdcmfuentedat, DbType.String, cagdcmfuentedat);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiGenerdemanDTO GetById(int cagdcmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cagdcmcodi, DbType.Int32, cagdcmcodi);
            CaiGenerdemanDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiGenerdemanDTO> List()
        {
            List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
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

        public List<CaiGenerdemanDTO> GetByCriteria()
        {
            List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
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

        public List<CaiGenerdemanDTO> GetByUsuarioLibresSGOCOES(string sFechaInicio, string sFechaFin, int iFormatcodi, int iTipoEmprcodi, string lectCodiPR16, string lectCodiAlpha)
        {
            List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
            string sqlQuery = string.Format(this.helper.SqlGetByUsuarioLibresSGOCOES, iFormatcodi, sFechaInicio, sFechaFin, iTipoEmprcodi, lectCodiPR16, lectCodiAlpha);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiGenerdemanDTO entity = new CaiGenerdemanDTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iCagdcmdia = dr.GetOrdinal(this.helper.Cagdcmdia);
                    if (!dr.IsDBNull(iCagdcmdia)) entity.Cagdcmdia = dr.GetDateTime(iCagdcmdia);

                    entity.Cagdcmtotaldia = 0;

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.Cagdcmtotaldia += entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.Cagdcmtotaldia += entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.Cagdcmtotaldia += entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.Cagdcmtotaldia += entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.Cagdcmtotaldia += entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.Cagdcmtotaldia += entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.Cagdcmtotaldia += entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.Cagdcmtotaldia += entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.Cagdcmtotaldia += entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.Cagdcmtotaldia += entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.Cagdcmtotaldia += entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.Cagdcmtotaldia += entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.Cagdcmtotaldia += entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.Cagdcmtotaldia += entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.Cagdcmtotaldia += entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.Cagdcmtotaldia += entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.Cagdcmtotaldia += entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.Cagdcmtotaldia += entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.Cagdcmtotaldia += entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.Cagdcmtotaldia += entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.Cagdcmtotaldia += entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.Cagdcmtotaldia += entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.Cagdcmtotaldia += entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.Cagdcmtotaldia += entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.Cagdcmtotaldia += entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.Cagdcmtotaldia += entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.Cagdcmtotaldia += entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.Cagdcmtotaldia += entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.Cagdcmtotaldia += entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.Cagdcmtotaldia += entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.Cagdcmtotaldia += entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.Cagdcmtotaldia += entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.Cagdcmtotaldia += entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.Cagdcmtotaldia += entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.Cagdcmtotaldia += entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.Cagdcmtotaldia += entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.Cagdcmtotaldia += entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.Cagdcmtotaldia += entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.Cagdcmtotaldia += entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.Cagdcmtotaldia += entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.Cagdcmtotaldia += entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.Cagdcmtotaldia += entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.Cagdcmtotaldia += entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.Cagdcmtotaldia += entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.Cagdcmtotaldia += entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.Cagdcmtotaldia += entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.Cagdcmtotaldia += entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.Cagdcmtotaldia += entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.Cagdcmtotaldia += entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.Cagdcmtotaldia += entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.Cagdcmtotaldia += entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.Cagdcmtotaldia += entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.Cagdcmtotaldia += entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.Cagdcmtotaldia += entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.Cagdcmtotaldia += entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.Cagdcmtotaldia += entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.Cagdcmtotaldia += entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.Cagdcmtotaldia += entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.Cagdcmtotaldia += entity.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(this.helper.H60);
                    if (!dr.IsDBNull(iH60)) entity.Cagdcmtotaldia += entity.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(this.helper.H61);
                    if (!dr.IsDBNull(iH61)) entity.Cagdcmtotaldia += entity.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(this.helper.H62);
                    if (!dr.IsDBNull(iH62)) entity.Cagdcmtotaldia += entity.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(this.helper.H63);
                    if (!dr.IsDBNull(iH63)) entity.Cagdcmtotaldia += entity.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(this.helper.H64);
                    if (!dr.IsDBNull(iH64)) entity.Cagdcmtotaldia += entity.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(this.helper.H65);
                    if (!dr.IsDBNull(iH65)) entity.Cagdcmtotaldia += entity.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(this.helper.H66);
                    if (!dr.IsDBNull(iH66)) entity.Cagdcmtotaldia += entity.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(this.helper.H67);
                    if (!dr.IsDBNull(iH67)) entity.Cagdcmtotaldia += entity.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(this.helper.H68);
                    if (!dr.IsDBNull(iH68)) entity.Cagdcmtotaldia += entity.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(this.helper.H69);
                    if (!dr.IsDBNull(iH69)) entity.Cagdcmtotaldia += entity.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(this.helper.H70);
                    if (!dr.IsDBNull(iH70)) entity.Cagdcmtotaldia += entity.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(this.helper.H71);
                    if (!dr.IsDBNull(iH71)) entity.Cagdcmtotaldia += entity.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(this.helper.H72);
                    if (!dr.IsDBNull(iH72)) entity.Cagdcmtotaldia += entity.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(this.helper.H73);
                    if (!dr.IsDBNull(iH73)) entity.Cagdcmtotaldia += entity.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(this.helper.H74);
                    if (!dr.IsDBNull(iH74)) entity.Cagdcmtotaldia += entity.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(this.helper.H75);
                    if (!dr.IsDBNull(iH75)) entity.Cagdcmtotaldia += entity.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(this.helper.H76);
                    if (!dr.IsDBNull(iH76)) entity.Cagdcmtotaldia += entity.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(this.helper.H77);
                    if (!dr.IsDBNull(iH77)) entity.Cagdcmtotaldia += entity.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(this.helper.H78);
                    if (!dr.IsDBNull(iH78)) entity.Cagdcmtotaldia += entity.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(this.helper.H79);
                    if (!dr.IsDBNull(iH79)) entity.Cagdcmtotaldia += entity.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(this.helper.H80);
                    if (!dr.IsDBNull(iH80)) entity.Cagdcmtotaldia += entity.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(this.helper.H81);
                    if (!dr.IsDBNull(iH81)) entity.Cagdcmtotaldia += entity.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(this.helper.H82);
                    if (!dr.IsDBNull(iH82)) entity.Cagdcmtotaldia += entity.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(this.helper.H83);
                    if (!dr.IsDBNull(iH83)) entity.Cagdcmtotaldia += entity.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(this.helper.H84);
                    if (!dr.IsDBNull(iH84)) entity.Cagdcmtotaldia += entity.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(this.helper.H85);
                    if (!dr.IsDBNull(iH85)) entity.Cagdcmtotaldia += entity.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(this.helper.H86);
                    if (!dr.IsDBNull(iH86)) entity.Cagdcmtotaldia += entity.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(this.helper.H87);
                    if (!dr.IsDBNull(iH87)) entity.Cagdcmtotaldia += entity.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(this.helper.H88);
                    if (!dr.IsDBNull(iH88)) entity.Cagdcmtotaldia += entity.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(this.helper.H89);
                    if (!dr.IsDBNull(iH89)) entity.Cagdcmtotaldia += entity.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(this.helper.H90);
                    if (!dr.IsDBNull(iH90)) entity.Cagdcmtotaldia += entity.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(this.helper.H91);
                    if (!dr.IsDBNull(iH91)) entity.Cagdcmtotaldia += entity.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(this.helper.H92);
                    if (!dr.IsDBNull(iH92)) entity.Cagdcmtotaldia += entity.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(this.helper.H93);
                    if (!dr.IsDBNull(iH93)) entity.Cagdcmtotaldia += entity.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(this.helper.H94);
                    if (!dr.IsDBNull(iH94)) entity.Cagdcmtotaldia += entity.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(this.helper.H95);
                    if (!dr.IsDBNull(iH95)) entity.Cagdcmtotaldia += entity.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(this.helper.H96);
                    if (!dr.IsDBNull(iH96)) entity.Cagdcmtotaldia += entity.H96 = dr.GetDecimal(iH96);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void BulkInsert(List<CaiGenerdemanDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Cagdcmcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Caiajcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cagdcmfuentedat, DbType.String);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodicen, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodiuni, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cagdcmcalidadinfo, DbType.String);
            dbProvider.AddColumnMapping(helper.Cagdcmdia, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Cagdcmtotaldia, DbType.Decimal);
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
            dbProvider.AddColumnMapping(helper.T1, DbType.String);
            dbProvider.AddColumnMapping(helper.T2, DbType.String);
            dbProvider.AddColumnMapping(helper.T3, DbType.String);
            dbProvider.AddColumnMapping(helper.T4, DbType.String);
            dbProvider.AddColumnMapping(helper.T5, DbType.String);
            dbProvider.AddColumnMapping(helper.T6, DbType.String);
            dbProvider.AddColumnMapping(helper.T7, DbType.String);
            dbProvider.AddColumnMapping(helper.T8, DbType.String);
            dbProvider.AddColumnMapping(helper.T9, DbType.String);
            dbProvider.AddColumnMapping(helper.T10, DbType.String);
            dbProvider.AddColumnMapping(helper.T11, DbType.String);
            dbProvider.AddColumnMapping(helper.T12, DbType.String);
            dbProvider.AddColumnMapping(helper.T13, DbType.String);
            dbProvider.AddColumnMapping(helper.T14, DbType.String);
            dbProvider.AddColumnMapping(helper.T15, DbType.String);
            dbProvider.AddColumnMapping(helper.T16, DbType.String);
            dbProvider.AddColumnMapping(helper.T17, DbType.String);
            dbProvider.AddColumnMapping(helper.T18, DbType.String);
            dbProvider.AddColumnMapping(helper.T19, DbType.String);
            dbProvider.AddColumnMapping(helper.T20, DbType.String);
            dbProvider.AddColumnMapping(helper.T21, DbType.String);
            dbProvider.AddColumnMapping(helper.T22, DbType.String);
            dbProvider.AddColumnMapping(helper.T23, DbType.String);
            dbProvider.AddColumnMapping(helper.T24, DbType.String);
            dbProvider.AddColumnMapping(helper.T25, DbType.String);
            dbProvider.AddColumnMapping(helper.T26, DbType.String);
            dbProvider.AddColumnMapping(helper.T27, DbType.String);
            dbProvider.AddColumnMapping(helper.T28, DbType.String);
            dbProvider.AddColumnMapping(helper.T29, DbType.String);
            dbProvider.AddColumnMapping(helper.T30, DbType.String);
            dbProvider.AddColumnMapping(helper.T31, DbType.String);
            dbProvider.AddColumnMapping(helper.T32, DbType.String);
            dbProvider.AddColumnMapping(helper.T33, DbType.String);
            dbProvider.AddColumnMapping(helper.T34, DbType.String);
            dbProvider.AddColumnMapping(helper.T35, DbType.String);
            dbProvider.AddColumnMapping(helper.T36, DbType.String);
            dbProvider.AddColumnMapping(helper.T37, DbType.String);
            dbProvider.AddColumnMapping(helper.T38, DbType.String);
            dbProvider.AddColumnMapping(helper.T39, DbType.String);
            dbProvider.AddColumnMapping(helper.T40, DbType.String);
            dbProvider.AddColumnMapping(helper.T41, DbType.String);
            dbProvider.AddColumnMapping(helper.T42, DbType.String);
            dbProvider.AddColumnMapping(helper.T43, DbType.String);
            dbProvider.AddColumnMapping(helper.T44, DbType.String);
            dbProvider.AddColumnMapping(helper.T45, DbType.String);
            dbProvider.AddColumnMapping(helper.T46, DbType.String);
            dbProvider.AddColumnMapping(helper.T47, DbType.String);
            dbProvider.AddColumnMapping(helper.T48, DbType.String);
            dbProvider.AddColumnMapping(helper.T49, DbType.String);
            dbProvider.AddColumnMapping(helper.T50, DbType.String);
            dbProvider.AddColumnMapping(helper.T51, DbType.String);
            dbProvider.AddColumnMapping(helper.T52, DbType.String);
            dbProvider.AddColumnMapping(helper.T53, DbType.String);
            dbProvider.AddColumnMapping(helper.T54, DbType.String);
            dbProvider.AddColumnMapping(helper.T55, DbType.String);
            dbProvider.AddColumnMapping(helper.T56, DbType.String);
            dbProvider.AddColumnMapping(helper.T57, DbType.String);
            dbProvider.AddColumnMapping(helper.T58, DbType.String);
            dbProvider.AddColumnMapping(helper.T59, DbType.String);
            dbProvider.AddColumnMapping(helper.T60, DbType.String);
            dbProvider.AddColumnMapping(helper.T61, DbType.String);
            dbProvider.AddColumnMapping(helper.T62, DbType.String);
            dbProvider.AddColumnMapping(helper.T63, DbType.String);
            dbProvider.AddColumnMapping(helper.T64, DbType.String);
            dbProvider.AddColumnMapping(helper.T65, DbType.String);
            dbProvider.AddColumnMapping(helper.T66, DbType.String);
            dbProvider.AddColumnMapping(helper.T67, DbType.String);
            dbProvider.AddColumnMapping(helper.T68, DbType.String);
            dbProvider.AddColumnMapping(helper.T69, DbType.String);
            dbProvider.AddColumnMapping(helper.T70, DbType.String);
            dbProvider.AddColumnMapping(helper.T71, DbType.String);
            dbProvider.AddColumnMapping(helper.T72, DbType.String);
            dbProvider.AddColumnMapping(helper.T73, DbType.String);
            dbProvider.AddColumnMapping(helper.T74, DbType.String);
            dbProvider.AddColumnMapping(helper.T75, DbType.String);
            dbProvider.AddColumnMapping(helper.T76, DbType.String);
            dbProvider.AddColumnMapping(helper.T77, DbType.String);
            dbProvider.AddColumnMapping(helper.T78, DbType.String);
            dbProvider.AddColumnMapping(helper.T79, DbType.String);
            dbProvider.AddColumnMapping(helper.T80, DbType.String);
            dbProvider.AddColumnMapping(helper.T81, DbType.String);
            dbProvider.AddColumnMapping(helper.T82, DbType.String);
            dbProvider.AddColumnMapping(helper.T83, DbType.String);
            dbProvider.AddColumnMapping(helper.T84, DbType.String);
            dbProvider.AddColumnMapping(helper.T85, DbType.String);
            dbProvider.AddColumnMapping(helper.T86, DbType.String);
            dbProvider.AddColumnMapping(helper.T87, DbType.String);
            dbProvider.AddColumnMapping(helper.T88, DbType.String);
            dbProvider.AddColumnMapping(helper.T89, DbType.String);
            dbProvider.AddColumnMapping(helper.T90, DbType.String);
            dbProvider.AddColumnMapping(helper.T91, DbType.String);
            dbProvider.AddColumnMapping(helper.T92, DbType.String);
            dbProvider.AddColumnMapping(helper.T93, DbType.String);
            dbProvider.AddColumnMapping(helper.T94, DbType.String);
            dbProvider.AddColumnMapping(helper.T95, DbType.String);
            dbProvider.AddColumnMapping(helper.T96, DbType.String);
            dbProvider.AddColumnMapping(helper.Cagdcmusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Cagdcmfeccreacion, DbType.DateTime);
            
            dbProvider.BulkInsert<CaiGenerdemanDTO>(entitys, helper.TableName);
        }

        public List<CaiGenerdemanDTO> ListGenDemBarrMes(int caiajcodi, int emprcodi, int ptomedicodi, int caajcmmes, int pericodi, int recacodi)
        {
            List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGenDemBarrMes);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            //Variables prestadas para la consulta
            dbProvider.AddInParameter(command, helper.Cagdcmcodi, DbType.Int32, caajcmmes);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, recacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiGenerdemanDTO entity = new CaiGenerdemanDTO();

                    int iCagdcmdia = dr.GetOrdinal(this.helper.Cagdcmdia);
                    if (!dr.IsDBNull(iCagdcmdia)) entity.Cagdcmdia = dr.GetDateTime(iCagdcmdia);

                    int iCagdcmfuentedat = dr.GetOrdinal(this.helper.Cagdcmfuentedat);
                    if (!dr.IsDBNull(iCagdcmfuentedat)) entity.Cagdcmfuentedat = dr.GetString(iCagdcmfuentedat);

                    int iCagdcmtotaldia = dr.GetOrdinal(this.helper.Cagdcmtotaldia);
                    if (!dr.IsDBNull(iCagdcmtotaldia)) entity.Cagdcmtotaldia = dr.GetDecimal(iCagdcmtotaldia);

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

                    int iCM1 = dr.GetOrdinal(this.helper.CM1);
                    if (!dr.IsDBNull(iCM1)) entity.CM1 = dr.GetDecimal(iCM1);

                    int iCM2 = dr.GetOrdinal(this.helper.CM2);
                    if (!dr.IsDBNull(iCM2)) entity.CM2 = dr.GetDecimal(iCM2);

                    int iCM3 = dr.GetOrdinal(this.helper.CM3);
                    if (!dr.IsDBNull(iCM3)) entity.CM3 = dr.GetDecimal(iCM3);

                    int iCM4 = dr.GetOrdinal(this.helper.CM4);
                    if (!dr.IsDBNull(iCM4)) entity.CM4 = dr.GetDecimal(iCM4);

                    int iCM5 = dr.GetOrdinal(this.helper.CM5);
                    if (!dr.IsDBNull(iCM5)) entity.CM5 = dr.GetDecimal(iCM5);

                    int iCM6 = dr.GetOrdinal(this.helper.CM6);
                    if (!dr.IsDBNull(iCM6)) entity.CM6 = dr.GetDecimal(iCM6);

                    int iCM7 = dr.GetOrdinal(this.helper.CM7);
                    if (!dr.IsDBNull(iCM7)) entity.CM7 = dr.GetDecimal(iCM7);

                    int iCM8 = dr.GetOrdinal(this.helper.CM8);
                    if (!dr.IsDBNull(iCM8)) entity.CM8 = dr.GetDecimal(iCM8);

                    int iCM9 = dr.GetOrdinal(this.helper.CM9);
                    if (!dr.IsDBNull(iCM9)) entity.CM9 = dr.GetDecimal(iCM9);

                    int iCM10 = dr.GetOrdinal(this.helper.CM10);
                    if (!dr.IsDBNull(iCM10)) entity.CM10 = dr.GetDecimal(iCM10);

                    int iCM11 = dr.GetOrdinal(this.helper.CM11);
                    if (!dr.IsDBNull(iCM11)) entity.CM11 = dr.GetDecimal(iCM11);

                    int iCM12 = dr.GetOrdinal(this.helper.CM12);
                    if (!dr.IsDBNull(iCM12)) entity.CM12 = dr.GetDecimal(iCM12);

                    int iCM13 = dr.GetOrdinal(this.helper.CM13);
                    if (!dr.IsDBNull(iCM13)) entity.CM13 = dr.GetDecimal(iCM13);

                    int iCM14 = dr.GetOrdinal(this.helper.CM14);
                    if (!dr.IsDBNull(iCM14)) entity.CM14 = dr.GetDecimal(iCM14);

                    int iCM15 = dr.GetOrdinal(this.helper.CM15);
                    if (!dr.IsDBNull(iCM15)) entity.CM15 = dr.GetDecimal(iCM15);

                    int iCM16 = dr.GetOrdinal(this.helper.CM16);
                    if (!dr.IsDBNull(iCM16)) entity.CM16 = dr.GetDecimal(iCM16);

                    int iCM17 = dr.GetOrdinal(this.helper.CM17);
                    if (!dr.IsDBNull(iCM17)) entity.CM17 = dr.GetDecimal(iCM17);

                    int iCM18 = dr.GetOrdinal(this.helper.CM18);
                    if (!dr.IsDBNull(iCM18)) entity.CM18 = dr.GetDecimal(iCM18);

                    int iCM19 = dr.GetOrdinal(this.helper.CM19);
                    if (!dr.IsDBNull(iCM19)) entity.CM19 = dr.GetDecimal(iCM19);

                    int iCM20 = dr.GetOrdinal(this.helper.CM20);
                    if (!dr.IsDBNull(iCM20)) entity.CM20 = dr.GetDecimal(iCM20);

                    int iCM21 = dr.GetOrdinal(this.helper.CM21);
                    if (!dr.IsDBNull(iCM21)) entity.CM21 = dr.GetDecimal(iCM21);

                    int iCM22 = dr.GetOrdinal(this.helper.CM22);
                    if (!dr.IsDBNull(iCM22)) entity.CM22 = dr.GetDecimal(iCM22);

                    int iCM23 = dr.GetOrdinal(this.helper.CM23);
                    if (!dr.IsDBNull(iCM23)) entity.CM23 = dr.GetDecimal(iCM23);

                    int iCM24 = dr.GetOrdinal(this.helper.CM24);
                    if (!dr.IsDBNull(iCM24)) entity.CM24 = dr.GetDecimal(iCM24);

                    int iCM25 = dr.GetOrdinal(this.helper.CM25);
                    if (!dr.IsDBNull(iCM25)) entity.CM25 = dr.GetDecimal(iCM25);

                    int iCM26 = dr.GetOrdinal(this.helper.CM26);
                    if (!dr.IsDBNull(iCM26)) entity.CM26 = dr.GetDecimal(iCM26);

                    int iCM27 = dr.GetOrdinal(this.helper.CM27);
                    if (!dr.IsDBNull(iCM27)) entity.CM27 = dr.GetDecimal(iCM27);

                    int iCM28 = dr.GetOrdinal(this.helper.CM28);
                    if (!dr.IsDBNull(iCM28)) entity.CM28 = dr.GetDecimal(iCM28);

                    int iCM29 = dr.GetOrdinal(this.helper.CM29);
                    if (!dr.IsDBNull(iCM29)) entity.CM29 = dr.GetDecimal(iCM29);

                    int iCM30 = dr.GetOrdinal(this.helper.CM30);
                    if (!dr.IsDBNull(iCM30)) entity.CM30 = dr.GetDecimal(iCM30);

                    int iCM31 = dr.GetOrdinal(this.helper.CM31);
                    if (!dr.IsDBNull(iCM31)) entity.CM31 = dr.GetDecimal(iCM31);

                    int iCM32 = dr.GetOrdinal(this.helper.CM32);
                    if (!dr.IsDBNull(iCM32)) entity.CM32 = dr.GetDecimal(iCM32);

                    int iCM33 = dr.GetOrdinal(this.helper.CM33);
                    if (!dr.IsDBNull(iCM33)) entity.CM33 = dr.GetDecimal(iCM33);

                    int iCM34 = dr.GetOrdinal(this.helper.CM34);
                    if (!dr.IsDBNull(iCM34)) entity.CM34 = dr.GetDecimal(iCM34);

                    int iCM35 = dr.GetOrdinal(this.helper.CM35);
                    if (!dr.IsDBNull(iCM35)) entity.CM35 = dr.GetDecimal(iCM35);

                    int iCM36 = dr.GetOrdinal(this.helper.CM36);
                    if (!dr.IsDBNull(iCM36)) entity.CM36 = dr.GetDecimal(iCM36);

                    int iCM37 = dr.GetOrdinal(this.helper.CM37);
                    if (!dr.IsDBNull(iCM37)) entity.CM37 = dr.GetDecimal(iCM37);

                    int iCM38 = dr.GetOrdinal(this.helper.CM38);
                    if (!dr.IsDBNull(iCM38)) entity.CM38 = dr.GetDecimal(iCM38);

                    int iCM39 = dr.GetOrdinal(this.helper.CM39);
                    if (!dr.IsDBNull(iCM39)) entity.CM39 = dr.GetDecimal(iCM39);

                    int iCM40 = dr.GetOrdinal(this.helper.CM40);
                    if (!dr.IsDBNull(iCM40)) entity.CM40 = dr.GetDecimal(iCM40);

                    int iCM41 = dr.GetOrdinal(this.helper.CM41);
                    if (!dr.IsDBNull(iCM41)) entity.CM41 = dr.GetDecimal(iCM41);

                    int iCM42 = dr.GetOrdinal(this.helper.CM42);
                    if (!dr.IsDBNull(iCM42)) entity.CM42 = dr.GetDecimal(iCM42);

                    int iCM43 = dr.GetOrdinal(this.helper.CM43);
                    if (!dr.IsDBNull(iCM43)) entity.CM43 = dr.GetDecimal(iCM43);

                    int iCM44 = dr.GetOrdinal(this.helper.CM44);
                    if (!dr.IsDBNull(iCM44)) entity.CM44 = dr.GetDecimal(iCM44);

                    int iCM45 = dr.GetOrdinal(this.helper.CM45);
                    if (!dr.IsDBNull(iCM45)) entity.CM45 = dr.GetDecimal(iCM45);

                    int iCM46 = dr.GetOrdinal(this.helper.CM46);
                    if (!dr.IsDBNull(iCM46)) entity.CM46 = dr.GetDecimal(iCM46);

                    int iCM47 = dr.GetOrdinal(this.helper.CM47);
                    if (!dr.IsDBNull(iCM47)) entity.CM47 = dr.GetDecimal(iCM47);

                    int iCM48 = dr.GetOrdinal(this.helper.CM48);
                    if (!dr.IsDBNull(iCM48)) entity.CM48 = dr.GetDecimal(iCM48);

                    int iCM49 = dr.GetOrdinal(this.helper.CM49);
                    if (!dr.IsDBNull(iCM49)) entity.CM49 = dr.GetDecimal(iCM49);

                    int iCM50 = dr.GetOrdinal(this.helper.CM50);
                    if (!dr.IsDBNull(iCM50)) entity.CM50 = dr.GetDecimal(iCM50);

                    int iCM51 = dr.GetOrdinal(this.helper.CM51);
                    if (!dr.IsDBNull(iCM51)) entity.CM51 = dr.GetDecimal(iCM51);

                    int iCM52 = dr.GetOrdinal(this.helper.CM52);
                    if (!dr.IsDBNull(iCM52)) entity.CM52 = dr.GetDecimal(iCM52);

                    int iCM53 = dr.GetOrdinal(this.helper.CM53);
                    if (!dr.IsDBNull(iCM53)) entity.CM53 = dr.GetDecimal(iCM53);

                    int iCM54 = dr.GetOrdinal(this.helper.CM54);
                    if (!dr.IsDBNull(iCM54)) entity.CM54 = dr.GetDecimal(iCM54);

                    int iCM55 = dr.GetOrdinal(this.helper.CM55);
                    if (!dr.IsDBNull(iCM55)) entity.CM55 = dr.GetDecimal(iCM55);

                    int iCM56 = dr.GetOrdinal(this.helper.CM56);
                    if (!dr.IsDBNull(iCM56)) entity.CM56 = dr.GetDecimal(iCM56);

                    int iCM57 = dr.GetOrdinal(this.helper.CM57);
                    if (!dr.IsDBNull(iCM57)) entity.CM57 = dr.GetDecimal(iCM57);

                    int iCM58 = dr.GetOrdinal(this.helper.CM58);
                    if (!dr.IsDBNull(iCM58)) entity.CM58 = dr.GetDecimal(iCM58);

                    int iCM59 = dr.GetOrdinal(this.helper.CM59);
                    if (!dr.IsDBNull(iCM59)) entity.CM59 = dr.GetDecimal(iCM59);

                    int iCM60 = dr.GetOrdinal(this.helper.CM60);
                    if (!dr.IsDBNull(iCM60)) entity.CM60 = dr.GetDecimal(iCM60);

                    int iCM61 = dr.GetOrdinal(this.helper.CM61);
                    if (!dr.IsDBNull(iCM61)) entity.CM61 = dr.GetDecimal(iCM61);

                    int iCM62 = dr.GetOrdinal(this.helper.CM62);
                    if (!dr.IsDBNull(iCM62)) entity.CM62 = dr.GetDecimal(iCM62);

                    int iCM63 = dr.GetOrdinal(this.helper.CM63);
                    if (!dr.IsDBNull(iCM63)) entity.CM63 = dr.GetDecimal(iCM63);

                    int iCM64 = dr.GetOrdinal(this.helper.CM64);
                    if (!dr.IsDBNull(iCM64)) entity.CM64 = dr.GetDecimal(iCM64);

                    int iCM65 = dr.GetOrdinal(this.helper.CM65);
                    if (!dr.IsDBNull(iCM65)) entity.CM65 = dr.GetDecimal(iCM65);

                    int iCM66 = dr.GetOrdinal(this.helper.CM66);
                    if (!dr.IsDBNull(iCM66)) entity.CM66 = dr.GetDecimal(iCM66);

                    int iCM67 = dr.GetOrdinal(this.helper.CM67);
                    if (!dr.IsDBNull(iCM67)) entity.CM67 = dr.GetDecimal(iCM67);

                    int iCM68 = dr.GetOrdinal(this.helper.CM68);
                    if (!dr.IsDBNull(iCM68)) entity.CM68 = dr.GetDecimal(iCM68);

                    int iCM69 = dr.GetOrdinal(this.helper.CM69);
                    if (!dr.IsDBNull(iCM69)) entity.CM69 = dr.GetDecimal(iCM69);

                    int iCM70 = dr.GetOrdinal(this.helper.CM70);
                    if (!dr.IsDBNull(iCM70)) entity.CM70 = dr.GetDecimal(iCM70);

                    int iCM71 = dr.GetOrdinal(this.helper.CM71);
                    if (!dr.IsDBNull(iCM71)) entity.CM71 = dr.GetDecimal(iCM71);

                    int iCM72 = dr.GetOrdinal(this.helper.CM72);
                    if (!dr.IsDBNull(iCM72)) entity.CM72 = dr.GetDecimal(iCM72);

                    int iCM73 = dr.GetOrdinal(this.helper.CM73);
                    if (!dr.IsDBNull(iCM73)) entity.CM73 = dr.GetDecimal(iCM73);

                    int iCM74 = dr.GetOrdinal(this.helper.CM74);
                    if (!dr.IsDBNull(iCM74)) entity.CM74 = dr.GetDecimal(iCM74);

                    int iCM75 = dr.GetOrdinal(this.helper.CM75);
                    if (!dr.IsDBNull(iCM75)) entity.CM75 = dr.GetDecimal(iCM75);

                    int iCM76 = dr.GetOrdinal(this.helper.CM76);
                    if (!dr.IsDBNull(iCM76)) entity.CM76 = dr.GetDecimal(iCM76);

                    int iCM77 = dr.GetOrdinal(this.helper.CM77);
                    if (!dr.IsDBNull(iCM77)) entity.CM77 = dr.GetDecimal(iCM77);

                    int iCM78 = dr.GetOrdinal(this.helper.CM78);
                    if (!dr.IsDBNull(iCM78)) entity.CM78 = dr.GetDecimal(iCM78);

                    int iCM79 = dr.GetOrdinal(this.helper.CM79);
                    if (!dr.IsDBNull(iCM79)) entity.CM79 = dr.GetDecimal(iCM79);

                    int iCM80 = dr.GetOrdinal(this.helper.CM80);
                    if (!dr.IsDBNull(iCM80)) entity.CM80 = dr.GetDecimal(iCM80);

                    int iCM81 = dr.GetOrdinal(this.helper.CM81);
                    if (!dr.IsDBNull(iCM81)) entity.CM81 = dr.GetDecimal(iCM81);

                    int iCM82 = dr.GetOrdinal(this.helper.CM82);
                    if (!dr.IsDBNull(iCM82)) entity.CM82 = dr.GetDecimal(iCM82);

                    int iCM83 = dr.GetOrdinal(this.helper.CM83);
                    if (!dr.IsDBNull(iCM83)) entity.CM83 = dr.GetDecimal(iCM83);

                    int iCM84 = dr.GetOrdinal(this.helper.CM84);
                    if (!dr.IsDBNull(iCM84)) entity.CM84 = dr.GetDecimal(iCM84);

                    int iCM85 = dr.GetOrdinal(this.helper.CM85);
                    if (!dr.IsDBNull(iCM85)) entity.CM85 = dr.GetDecimal(iCM85);

                    int iCM86 = dr.GetOrdinal(this.helper.CM86);
                    if (!dr.IsDBNull(iCM86)) entity.CM86 = dr.GetDecimal(iCM86);

                    int iCM87 = dr.GetOrdinal(this.helper.CM87);
                    if (!dr.IsDBNull(iCM87)) entity.CM87 = dr.GetDecimal(iCM87);

                    int iCM88 = dr.GetOrdinal(this.helper.CM88);
                    if (!dr.IsDBNull(iCM88)) entity.CM88 = dr.GetDecimal(iCM88);

                    int iCM89 = dr.GetOrdinal(this.helper.CM89);
                    if (!dr.IsDBNull(iCM89)) entity.CM89 = dr.GetDecimal(iCM89);

                    int iCM90 = dr.GetOrdinal(this.helper.CM90);
                    if (!dr.IsDBNull(iCM90)) entity.CM90 = dr.GetDecimal(iCM90);

                    int iCM91 = dr.GetOrdinal(this.helper.CM91);
                    if (!dr.IsDBNull(iCM91)) entity.CM91 = dr.GetDecimal(iCM91);

                    int iCM92 = dr.GetOrdinal(this.helper.CM92);
                    if (!dr.IsDBNull(iCM92)) entity.CM92 = dr.GetDecimal(iCM92);

                    int iCM93 = dr.GetOrdinal(this.helper.CM93);
                    if (!dr.IsDBNull(iCM93)) entity.CM93 = dr.GetDecimal(iCM93);

                    int iCM94 = dr.GetOrdinal(this.helper.CM94);
                    if (!dr.IsDBNull(iCM94)) entity.CM94 = dr.GetDecimal(iCM94);

                    int iCM95 = dr.GetOrdinal(this.helper.CM95);
                    if (!dr.IsDBNull(iCM95)) entity.CM95 = dr.GetDecimal(iCM95);

                    int iCM96 = dr.GetOrdinal(this.helper.CM96);
                    if (!dr.IsDBNull(iCM96)) entity.CM96 = dr.GetDecimal(iCM96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CaiGenerdemanDTO> ListGenDemProyMes(int caiajcodi, int emprcodi, int ptomedicodi, int caajcmmes)
        {
            List<CaiGenerdemanDTO> entitys = new List<CaiGenerdemanDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGenDemProyMes);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            //Variables prestadas para la consulta
            dbProvider.AddInParameter(command, helper.Cagdcmcodi, DbType.Int32, caajcmmes);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiGenerdemanDTO entity = new CaiGenerdemanDTO();

                    int iCagdcmdia = dr.GetOrdinal(this.helper.Cagdcmdia);
                    if (!dr.IsDBNull(iCagdcmdia)) entity.Cagdcmdia = dr.GetDateTime(iCagdcmdia);

                    int iCagdcmfuentedat = dr.GetOrdinal(this.helper.Cagdcmfuentedat);
                    if (!dr.IsDBNull(iCagdcmfuentedat)) entity.Cagdcmfuentedat = dr.GetString(iCagdcmfuentedat);

                    int iCagdcmtotaldia = dr.GetOrdinal(this.helper.Cagdcmtotaldia);
                    if (!dr.IsDBNull(iCagdcmtotaldia)) entity.Cagdcmtotaldia = dr.GetDecimal(iCagdcmtotaldia);

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

                    int iCM1 = dr.GetOrdinal(this.helper.CM1);
                    if (!dr.IsDBNull(iCM1)) entity.CM1 = dr.GetDecimal(iCM1);

                    int iCM2 = dr.GetOrdinal(this.helper.CM2);
                    if (!dr.IsDBNull(iCM2)) entity.CM2 = dr.GetDecimal(iCM2);

                    int iCM3 = dr.GetOrdinal(this.helper.CM3);
                    if (!dr.IsDBNull(iCM3)) entity.CM3 = dr.GetDecimal(iCM3);

                    int iCM4 = dr.GetOrdinal(this.helper.CM4);
                    if (!dr.IsDBNull(iCM4)) entity.CM4 = dr.GetDecimal(iCM4);

                    int iCM5 = dr.GetOrdinal(this.helper.CM5);
                    if (!dr.IsDBNull(iCM5)) entity.CM5 = dr.GetDecimal(iCM5);

                    int iCM6 = dr.GetOrdinal(this.helper.CM6);
                    if (!dr.IsDBNull(iCM6)) entity.CM6 = dr.GetDecimal(iCM6);

                    int iCM7 = dr.GetOrdinal(this.helper.CM7);
                    if (!dr.IsDBNull(iCM7)) entity.CM7 = dr.GetDecimal(iCM7);

                    int iCM8 = dr.GetOrdinal(this.helper.CM8);
                    if (!dr.IsDBNull(iCM8)) entity.CM8 = dr.GetDecimal(iCM8);

                    int iCM9 = dr.GetOrdinal(this.helper.CM9);
                    if (!dr.IsDBNull(iCM9)) entity.CM9 = dr.GetDecimal(iCM9);

                    int iCM10 = dr.GetOrdinal(this.helper.CM10);
                    if (!dr.IsDBNull(iCM10)) entity.CM10 = dr.GetDecimal(iCM10);

                    int iCM11 = dr.GetOrdinal(this.helper.CM11);
                    if (!dr.IsDBNull(iCM11)) entity.CM11 = dr.GetDecimal(iCM11);

                    int iCM12 = dr.GetOrdinal(this.helper.CM12);
                    if (!dr.IsDBNull(iCM12)) entity.CM12 = dr.GetDecimal(iCM12);

                    int iCM13 = dr.GetOrdinal(this.helper.CM13);
                    if (!dr.IsDBNull(iCM13)) entity.CM13 = dr.GetDecimal(iCM13);

                    int iCM14 = dr.GetOrdinal(this.helper.CM14);
                    if (!dr.IsDBNull(iCM14)) entity.CM14 = dr.GetDecimal(iCM14);

                    int iCM15 = dr.GetOrdinal(this.helper.CM15);
                    if (!dr.IsDBNull(iCM15)) entity.CM15 = dr.GetDecimal(iCM15);

                    int iCM16 = dr.GetOrdinal(this.helper.CM16);
                    if (!dr.IsDBNull(iCM16)) entity.CM16 = dr.GetDecimal(iCM16);

                    int iCM17 = dr.GetOrdinal(this.helper.CM17);
                    if (!dr.IsDBNull(iCM17)) entity.CM17 = dr.GetDecimal(iCM17);

                    int iCM18 = dr.GetOrdinal(this.helper.CM18);
                    if (!dr.IsDBNull(iCM18)) entity.CM18 = dr.GetDecimal(iCM18);

                    int iCM19 = dr.GetOrdinal(this.helper.CM19);
                    if (!dr.IsDBNull(iCM19)) entity.CM19 = dr.GetDecimal(iCM19);

                    int iCM20 = dr.GetOrdinal(this.helper.CM20);
                    if (!dr.IsDBNull(iCM20)) entity.CM20 = dr.GetDecimal(iCM20);

                    int iCM21 = dr.GetOrdinal(this.helper.CM21);
                    if (!dr.IsDBNull(iCM21)) entity.CM21 = dr.GetDecimal(iCM21);

                    int iCM22 = dr.GetOrdinal(this.helper.CM22);
                    if (!dr.IsDBNull(iCM22)) entity.CM22 = dr.GetDecimal(iCM22);

                    int iCM23 = dr.GetOrdinal(this.helper.CM23);
                    if (!dr.IsDBNull(iCM23)) entity.CM23 = dr.GetDecimal(iCM23);

                    int iCM24 = dr.GetOrdinal(this.helper.CM24);
                    if (!dr.IsDBNull(iCM24)) entity.CM24 = dr.GetDecimal(iCM24);

                    int iCM25 = dr.GetOrdinal(this.helper.CM25);
                    if (!dr.IsDBNull(iCM25)) entity.CM25 = dr.GetDecimal(iCM25);

                    int iCM26 = dr.GetOrdinal(this.helper.CM26);
                    if (!dr.IsDBNull(iCM26)) entity.CM26 = dr.GetDecimal(iCM26);

                    int iCM27 = dr.GetOrdinal(this.helper.CM27);
                    if (!dr.IsDBNull(iCM27)) entity.CM27 = dr.GetDecimal(iCM27);

                    int iCM28 = dr.GetOrdinal(this.helper.CM28);
                    if (!dr.IsDBNull(iCM28)) entity.CM28 = dr.GetDecimal(iCM28);

                    int iCM29 = dr.GetOrdinal(this.helper.CM29);
                    if (!dr.IsDBNull(iCM29)) entity.CM29 = dr.GetDecimal(iCM29);

                    int iCM30 = dr.GetOrdinal(this.helper.CM30);
                    if (!dr.IsDBNull(iCM30)) entity.CM30 = dr.GetDecimal(iCM30);

                    int iCM31 = dr.GetOrdinal(this.helper.CM31);
                    if (!dr.IsDBNull(iCM31)) entity.CM31 = dr.GetDecimal(iCM31);

                    int iCM32 = dr.GetOrdinal(this.helper.CM32);
                    if (!dr.IsDBNull(iCM32)) entity.CM32 = dr.GetDecimal(iCM32);

                    int iCM33 = dr.GetOrdinal(this.helper.CM33);
                    if (!dr.IsDBNull(iCM33)) entity.CM33 = dr.GetDecimal(iCM33);

                    int iCM34 = dr.GetOrdinal(this.helper.CM34);
                    if (!dr.IsDBNull(iCM34)) entity.CM34 = dr.GetDecimal(iCM34);

                    int iCM35 = dr.GetOrdinal(this.helper.CM35);
                    if (!dr.IsDBNull(iCM35)) entity.CM35 = dr.GetDecimal(iCM35);

                    int iCM36 = dr.GetOrdinal(this.helper.CM36);
                    if (!dr.IsDBNull(iCM36)) entity.CM36 = dr.GetDecimal(iCM36);

                    int iCM37 = dr.GetOrdinal(this.helper.CM37);
                    if (!dr.IsDBNull(iCM37)) entity.CM37 = dr.GetDecimal(iCM37);

                    int iCM38 = dr.GetOrdinal(this.helper.CM38);
                    if (!dr.IsDBNull(iCM38)) entity.CM38 = dr.GetDecimal(iCM38);

                    int iCM39 = dr.GetOrdinal(this.helper.CM39);
                    if (!dr.IsDBNull(iCM39)) entity.CM39 = dr.GetDecimal(iCM39);

                    int iCM40 = dr.GetOrdinal(this.helper.CM40);
                    if (!dr.IsDBNull(iCM40)) entity.CM40 = dr.GetDecimal(iCM40);

                    int iCM41 = dr.GetOrdinal(this.helper.CM41);
                    if (!dr.IsDBNull(iCM41)) entity.CM41 = dr.GetDecimal(iCM41);

                    int iCM42 = dr.GetOrdinal(this.helper.CM42);
                    if (!dr.IsDBNull(iCM42)) entity.CM42 = dr.GetDecimal(iCM42);

                    int iCM43 = dr.GetOrdinal(this.helper.CM43);
                    if (!dr.IsDBNull(iCM43)) entity.CM43 = dr.GetDecimal(iCM43);

                    int iCM44 = dr.GetOrdinal(this.helper.CM44);
                    if (!dr.IsDBNull(iCM44)) entity.CM44 = dr.GetDecimal(iCM44);

                    int iCM45 = dr.GetOrdinal(this.helper.CM45);
                    if (!dr.IsDBNull(iCM45)) entity.CM45 = dr.GetDecimal(iCM45);

                    int iCM46 = dr.GetOrdinal(this.helper.CM46);
                    if (!dr.IsDBNull(iCM46)) entity.CM46 = dr.GetDecimal(iCM46);

                    int iCM47 = dr.GetOrdinal(this.helper.CM47);
                    if (!dr.IsDBNull(iCM47)) entity.CM47 = dr.GetDecimal(iCM47);

                    int iCM48 = dr.GetOrdinal(this.helper.CM48);
                    if (!dr.IsDBNull(iCM48)) entity.CM48 = dr.GetDecimal(iCM48);

                    int iCM49 = dr.GetOrdinal(this.helper.CM49);
                    if (!dr.IsDBNull(iCM49)) entity.CM49 = dr.GetDecimal(iCM49);

                    int iCM50 = dr.GetOrdinal(this.helper.CM50);
                    if (!dr.IsDBNull(iCM50)) entity.CM50 = dr.GetDecimal(iCM50);

                    int iCM51 = dr.GetOrdinal(this.helper.CM51);
                    if (!dr.IsDBNull(iCM51)) entity.CM51 = dr.GetDecimal(iCM51);

                    int iCM52 = dr.GetOrdinal(this.helper.CM52);
                    if (!dr.IsDBNull(iCM52)) entity.CM52 = dr.GetDecimal(iCM52);

                    int iCM53 = dr.GetOrdinal(this.helper.CM53);
                    if (!dr.IsDBNull(iCM53)) entity.CM53 = dr.GetDecimal(iCM53);

                    int iCM54 = dr.GetOrdinal(this.helper.CM54);
                    if (!dr.IsDBNull(iCM54)) entity.CM54 = dr.GetDecimal(iCM54);

                    int iCM55 = dr.GetOrdinal(this.helper.CM55);
                    if (!dr.IsDBNull(iCM55)) entity.CM55 = dr.GetDecimal(iCM55);

                    int iCM56 = dr.GetOrdinal(this.helper.CM56);
                    if (!dr.IsDBNull(iCM56)) entity.CM56 = dr.GetDecimal(iCM56);

                    int iCM57 = dr.GetOrdinal(this.helper.CM57);
                    if (!dr.IsDBNull(iCM57)) entity.CM57 = dr.GetDecimal(iCM57);

                    int iCM58 = dr.GetOrdinal(this.helper.CM58);
                    if (!dr.IsDBNull(iCM58)) entity.CM58 = dr.GetDecimal(iCM58);

                    int iCM59 = dr.GetOrdinal(this.helper.CM59);
                    if (!dr.IsDBNull(iCM59)) entity.CM59 = dr.GetDecimal(iCM59);

                    int iCM60 = dr.GetOrdinal(this.helper.CM60);
                    if (!dr.IsDBNull(iCM60)) entity.CM60 = dr.GetDecimal(iCM60);

                    int iCM61 = dr.GetOrdinal(this.helper.CM61);
                    if (!dr.IsDBNull(iCM61)) entity.CM61 = dr.GetDecimal(iCM61);

                    int iCM62 = dr.GetOrdinal(this.helper.CM62);
                    if (!dr.IsDBNull(iCM62)) entity.CM62 = dr.GetDecimal(iCM62);

                    int iCM63 = dr.GetOrdinal(this.helper.CM63);
                    if (!dr.IsDBNull(iCM63)) entity.CM63 = dr.GetDecimal(iCM63);

                    int iCM64 = dr.GetOrdinal(this.helper.CM64);
                    if (!dr.IsDBNull(iCM64)) entity.CM64 = dr.GetDecimal(iCM64);

                    int iCM65 = dr.GetOrdinal(this.helper.CM65);
                    if (!dr.IsDBNull(iCM65)) entity.CM65 = dr.GetDecimal(iCM65);

                    int iCM66 = dr.GetOrdinal(this.helper.CM66);
                    if (!dr.IsDBNull(iCM66)) entity.CM66 = dr.GetDecimal(iCM66);

                    int iCM67 = dr.GetOrdinal(this.helper.CM67);
                    if (!dr.IsDBNull(iCM67)) entity.CM67 = dr.GetDecimal(iCM67);

                    int iCM68 = dr.GetOrdinal(this.helper.CM68);
                    if (!dr.IsDBNull(iCM68)) entity.CM68 = dr.GetDecimal(iCM68);

                    int iCM69 = dr.GetOrdinal(this.helper.CM69);
                    if (!dr.IsDBNull(iCM69)) entity.CM69 = dr.GetDecimal(iCM69);

                    int iCM70 = dr.GetOrdinal(this.helper.CM70);
                    if (!dr.IsDBNull(iCM70)) entity.CM70 = dr.GetDecimal(iCM70);

                    int iCM71 = dr.GetOrdinal(this.helper.CM71);
                    if (!dr.IsDBNull(iCM71)) entity.CM71 = dr.GetDecimal(iCM71);

                    int iCM72 = dr.GetOrdinal(this.helper.CM72);
                    if (!dr.IsDBNull(iCM72)) entity.CM72 = dr.GetDecimal(iCM72);

                    int iCM73 = dr.GetOrdinal(this.helper.CM73);
                    if (!dr.IsDBNull(iCM73)) entity.CM73 = dr.GetDecimal(iCM73);

                    int iCM74 = dr.GetOrdinal(this.helper.CM74);
                    if (!dr.IsDBNull(iCM74)) entity.CM74 = dr.GetDecimal(iCM74);

                    int iCM75 = dr.GetOrdinal(this.helper.CM75);
                    if (!dr.IsDBNull(iCM75)) entity.CM75 = dr.GetDecimal(iCM75);

                    int iCM76 = dr.GetOrdinal(this.helper.CM76);
                    if (!dr.IsDBNull(iCM76)) entity.CM76 = dr.GetDecimal(iCM76);

                    int iCM77 = dr.GetOrdinal(this.helper.CM77);
                    if (!dr.IsDBNull(iCM77)) entity.CM77 = dr.GetDecimal(iCM77);

                    int iCM78 = dr.GetOrdinal(this.helper.CM78);
                    if (!dr.IsDBNull(iCM78)) entity.CM78 = dr.GetDecimal(iCM78);

                    int iCM79 = dr.GetOrdinal(this.helper.CM79);
                    if (!dr.IsDBNull(iCM79)) entity.CM79 = dr.GetDecimal(iCM79);

                    int iCM80 = dr.GetOrdinal(this.helper.CM80);
                    if (!dr.IsDBNull(iCM80)) entity.CM80 = dr.GetDecimal(iCM80);

                    int iCM81 = dr.GetOrdinal(this.helper.CM81);
                    if (!dr.IsDBNull(iCM81)) entity.CM81 = dr.GetDecimal(iCM81);

                    int iCM82 = dr.GetOrdinal(this.helper.CM82);
                    if (!dr.IsDBNull(iCM82)) entity.CM82 = dr.GetDecimal(iCM82);

                    int iCM83 = dr.GetOrdinal(this.helper.CM83);
                    if (!dr.IsDBNull(iCM83)) entity.CM83 = dr.GetDecimal(iCM83);

                    int iCM84 = dr.GetOrdinal(this.helper.CM84);
                    if (!dr.IsDBNull(iCM84)) entity.CM84 = dr.GetDecimal(iCM84);

                    int iCM85 = dr.GetOrdinal(this.helper.CM85);
                    if (!dr.IsDBNull(iCM85)) entity.CM85 = dr.GetDecimal(iCM85);

                    int iCM86 = dr.GetOrdinal(this.helper.CM86);
                    if (!dr.IsDBNull(iCM86)) entity.CM86 = dr.GetDecimal(iCM86);

                    int iCM87 = dr.GetOrdinal(this.helper.CM87);
                    if (!dr.IsDBNull(iCM87)) entity.CM87 = dr.GetDecimal(iCM87);

                    int iCM88 = dr.GetOrdinal(this.helper.CM88);
                    if (!dr.IsDBNull(iCM88)) entity.CM88 = dr.GetDecimal(iCM88);

                    int iCM89 = dr.GetOrdinal(this.helper.CM89);
                    if (!dr.IsDBNull(iCM89)) entity.CM89 = dr.GetDecimal(iCM89);

                    int iCM90 = dr.GetOrdinal(this.helper.CM90);
                    if (!dr.IsDBNull(iCM90)) entity.CM90 = dr.GetDecimal(iCM90);

                    int iCM91 = dr.GetOrdinal(this.helper.CM91);
                    if (!dr.IsDBNull(iCM91)) entity.CM91 = dr.GetDecimal(iCM91);

                    int iCM92 = dr.GetOrdinal(this.helper.CM92);
                    if (!dr.IsDBNull(iCM92)) entity.CM92 = dr.GetDecimal(iCM92);

                    int iCM93 = dr.GetOrdinal(this.helper.CM93);
                    if (!dr.IsDBNull(iCM93)) entity.CM93 = dr.GetDecimal(iCM93);

                    int iCM94 = dr.GetOrdinal(this.helper.CM94);
                    if (!dr.IsDBNull(iCM94)) entity.CM94 = dr.GetDecimal(iCM94);

                    int iCM95 = dr.GetOrdinal(this.helper.CM95);
                    if (!dr.IsDBNull(iCM95)) entity.CM95 = dr.GetDecimal(iCM95);

                    int iCM96 = dr.GetOrdinal(this.helper.CM96);
                    if (!dr.IsDBNull(iCM96)) entity.CM96 = dr.GetDecimal(iCM96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    
    }
}

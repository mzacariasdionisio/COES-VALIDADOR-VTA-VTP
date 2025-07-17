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
    public class VceEnergiaRepository : RepositoryBase, IVceEnergiaRepository
    {
        public VceEnergiaRepository(string strConn): base(strConn)
        {
        }

        VceEnergiaHelper helper = new VceEnergiaHelper();

        public void Save(VceEnergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crmeh47, DbType.Decimal, entity.Crmeh47);
            dbProvider.AddInParameter(command, helper.Crmeh46, DbType.Decimal, entity.Crmeh46);
            dbProvider.AddInParameter(command, helper.Crmeh45, DbType.Decimal, entity.Crmeh45);
            dbProvider.AddInParameter(command, helper.Crmeh44, DbType.Decimal, entity.Crmeh44);
            dbProvider.AddInParameter(command, helper.Crmeh43, DbType.Decimal, entity.Crmeh43);
            dbProvider.AddInParameter(command, helper.Crmeh42, DbType.Decimal, entity.Crmeh42);
            dbProvider.AddInParameter(command, helper.Crmeh41, DbType.Decimal, entity.Crmeh41);
            dbProvider.AddInParameter(command, helper.Crmeh40, DbType.Decimal, entity.Crmeh40);
            dbProvider.AddInParameter(command, helper.Crmeh39, DbType.Decimal, entity.Crmeh39);
            dbProvider.AddInParameter(command, helper.Crmeh38, DbType.Decimal, entity.Crmeh38);
            dbProvider.AddInParameter(command, helper.Crmeh37, DbType.Decimal, entity.Crmeh37);
            dbProvider.AddInParameter(command, helper.Crmeh36, DbType.Decimal, entity.Crmeh36);
            dbProvider.AddInParameter(command, helper.Crmeh35, DbType.Decimal, entity.Crmeh35);
            dbProvider.AddInParameter(command, helper.Crmeh34, DbType.Decimal, entity.Crmeh34);
            dbProvider.AddInParameter(command, helper.Crmeh33, DbType.Decimal, entity.Crmeh33);
            dbProvider.AddInParameter(command, helper.Crmeh32, DbType.Decimal, entity.Crmeh32);
            dbProvider.AddInParameter(command, helper.Crmeh31, DbType.Decimal, entity.Crmeh31);
            dbProvider.AddInParameter(command, helper.Crmeh30, DbType.Decimal, entity.Crmeh30);
            dbProvider.AddInParameter(command, helper.Crmeh29, DbType.Decimal, entity.Crmeh29);
            dbProvider.AddInParameter(command, helper.Crmeh28, DbType.Decimal, entity.Crmeh28);
            dbProvider.AddInParameter(command, helper.Crmeh27, DbType.Decimal, entity.Crmeh27);
            dbProvider.AddInParameter(command, helper.Crmeh26, DbType.Decimal, entity.Crmeh26);
            dbProvider.AddInParameter(command, helper.Crmeh25, DbType.Decimal, entity.Crmeh25);
            dbProvider.AddInParameter(command, helper.Crmeh24, DbType.Decimal, entity.Crmeh24);
            dbProvider.AddInParameter(command, helper.Crmeh23, DbType.Decimal, entity.Crmeh23);
            dbProvider.AddInParameter(command, helper.Crmeh22, DbType.Decimal, entity.Crmeh22);
            dbProvider.AddInParameter(command, helper.Crmeh21, DbType.Decimal, entity.Crmeh21);
            dbProvider.AddInParameter(command, helper.Crmeh20, DbType.Decimal, entity.Crmeh20);
            dbProvider.AddInParameter(command, helper.Crmeh19, DbType.Decimal, entity.Crmeh19);
            dbProvider.AddInParameter(command, helper.Crmeh18, DbType.Decimal, entity.Crmeh18);
            dbProvider.AddInParameter(command, helper.Crmeh17, DbType.Decimal, entity.Crmeh17);
            dbProvider.AddInParameter(command, helper.Crmeh16, DbType.Decimal, entity.Crmeh16);
            dbProvider.AddInParameter(command, helper.Crmeh15, DbType.Decimal, entity.Crmeh15);
            dbProvider.AddInParameter(command, helper.Crmeh14, DbType.Decimal, entity.Crmeh14);
            dbProvider.AddInParameter(command, helper.Crmeh13, DbType.Decimal, entity.Crmeh13);
            dbProvider.AddInParameter(command, helper.Crmeh12, DbType.Decimal, entity.Crmeh12);
            dbProvider.AddInParameter(command, helper.Crmeh11, DbType.Decimal, entity.Crmeh11);
            dbProvider.AddInParameter(command, helper.Crmeh10, DbType.Decimal, entity.Crmeh10);
            dbProvider.AddInParameter(command, helper.Crmeh9, DbType.Decimal, entity.Crmeh9);
            dbProvider.AddInParameter(command, helper.Crmeh8, DbType.Decimal, entity.Crmeh8);
            dbProvider.AddInParameter(command, helper.Crmeh7, DbType.Decimal, entity.Crmeh7);
            dbProvider.AddInParameter(command, helper.Crmeh6, DbType.Decimal, entity.Crmeh6);
            dbProvider.AddInParameter(command, helper.Crmeh5, DbType.Decimal, entity.Crmeh5);
            dbProvider.AddInParameter(command, helper.Crmeh4, DbType.Decimal, entity.Crmeh4);
            dbProvider.AddInParameter(command, helper.Crmeh3, DbType.Decimal, entity.Crmeh3);
            dbProvider.AddInParameter(command, helper.Crmeh2, DbType.Decimal, entity.Crmeh2);
            dbProvider.AddInParameter(command, helper.Crmeh1, DbType.Decimal, entity.Crmeh1);
            dbProvider.AddInParameter(command, helper.Crmemeditotal, DbType.Decimal, entity.Crmemeditotal);
            dbProvider.AddInParameter(command, helper.Crmefecha, DbType.DateTime, entity.Crmefecha);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Crmeh96, DbType.Decimal, entity.Crmeh96);
            dbProvider.AddInParameter(command, helper.Crmeh95, DbType.Decimal, entity.Crmeh95);
            dbProvider.AddInParameter(command, helper.Crmeh94, DbType.Decimal, entity.Crmeh94);
            dbProvider.AddInParameter(command, helper.Crmeh93, DbType.Decimal, entity.Crmeh93);
            dbProvider.AddInParameter(command, helper.Crmeh92, DbType.Decimal, entity.Crmeh92);
            dbProvider.AddInParameter(command, helper.Crmeh91, DbType.Decimal, entity.Crmeh91);
            dbProvider.AddInParameter(command, helper.Crmeh90, DbType.Decimal, entity.Crmeh90);
            dbProvider.AddInParameter(command, helper.Crmeh89, DbType.Decimal, entity.Crmeh89);
            dbProvider.AddInParameter(command, helper.Crmeh88, DbType.Decimal, entity.Crmeh88);
            dbProvider.AddInParameter(command, helper.Crmeh87, DbType.Decimal, entity.Crmeh87);
            dbProvider.AddInParameter(command, helper.Crmeh86, DbType.Decimal, entity.Crmeh86);
            dbProvider.AddInParameter(command, helper.Crmeh85, DbType.Decimal, entity.Crmeh85);
            dbProvider.AddInParameter(command, helper.Crmeh84, DbType.Decimal, entity.Crmeh84);
            dbProvider.AddInParameter(command, helper.Crmeh83, DbType.Decimal, entity.Crmeh83);
            dbProvider.AddInParameter(command, helper.Crmeh82, DbType.Decimal, entity.Crmeh82);
            dbProvider.AddInParameter(command, helper.Crmeh81, DbType.Decimal, entity.Crmeh81);
            dbProvider.AddInParameter(command, helper.Crmeh80, DbType.Decimal, entity.Crmeh80);
            dbProvider.AddInParameter(command, helper.Crmeh79, DbType.Decimal, entity.Crmeh79);
            dbProvider.AddInParameter(command, helper.Crmeh78, DbType.Decimal, entity.Crmeh78);
            dbProvider.AddInParameter(command, helper.Crmeh77, DbType.Decimal, entity.Crmeh77);
            dbProvider.AddInParameter(command, helper.Crmeh76, DbType.Decimal, entity.Crmeh76);
            dbProvider.AddInParameter(command, helper.Crmeh75, DbType.Decimal, entity.Crmeh75);
            dbProvider.AddInParameter(command, helper.Crmeh74, DbType.Decimal, entity.Crmeh74);
            dbProvider.AddInParameter(command, helper.Crmeh73, DbType.Decimal, entity.Crmeh73);
            dbProvider.AddInParameter(command, helper.Crmeh72, DbType.Decimal, entity.Crmeh72);
            dbProvider.AddInParameter(command, helper.Crmeh71, DbType.Decimal, entity.Crmeh71);
            dbProvider.AddInParameter(command, helper.Crmeh70, DbType.Decimal, entity.Crmeh70);
            dbProvider.AddInParameter(command, helper.Crmeh69, DbType.Decimal, entity.Crmeh69);
            dbProvider.AddInParameter(command, helper.Crmeh68, DbType.Decimal, entity.Crmeh68);
            dbProvider.AddInParameter(command, helper.Crmeh67, DbType.Decimal, entity.Crmeh67);
            dbProvider.AddInParameter(command, helper.Crmeh66, DbType.Decimal, entity.Crmeh66);
            dbProvider.AddInParameter(command, helper.Crmeh65, DbType.Decimal, entity.Crmeh65);
            dbProvider.AddInParameter(command, helper.Crmeh64, DbType.Decimal, entity.Crmeh64);
            dbProvider.AddInParameter(command, helper.Crmeh63, DbType.Decimal, entity.Crmeh63);
            dbProvider.AddInParameter(command, helper.Crmeh62, DbType.Decimal, entity.Crmeh62);
            dbProvider.AddInParameter(command, helper.Crmeh61, DbType.Decimal, entity.Crmeh61);
            dbProvider.AddInParameter(command, helper.Crmeh60, DbType.Decimal, entity.Crmeh60);
            dbProvider.AddInParameter(command, helper.Crmeh59, DbType.Decimal, entity.Crmeh59);
            dbProvider.AddInParameter(command, helper.Crmeh58, DbType.Decimal, entity.Crmeh58);
            dbProvider.AddInParameter(command, helper.Crmeh57, DbType.Decimal, entity.Crmeh57);
            dbProvider.AddInParameter(command, helper.Crmeh56, DbType.Decimal, entity.Crmeh56);
            dbProvider.AddInParameter(command, helper.Crmeh55, DbType.Decimal, entity.Crmeh55);
            dbProvider.AddInParameter(command, helper.Crmeh54, DbType.Decimal, entity.Crmeh54);
            dbProvider.AddInParameter(command, helper.Crmeh53, DbType.Decimal, entity.Crmeh53);
            dbProvider.AddInParameter(command, helper.Crmeh52, DbType.Decimal, entity.Crmeh52);
            dbProvider.AddInParameter(command, helper.Crmeh51, DbType.Decimal, entity.Crmeh51);
            dbProvider.AddInParameter(command, helper.Crmeh50, DbType.Decimal, entity.Crmeh50);
            dbProvider.AddInParameter(command, helper.Crmeh49, DbType.Decimal, entity.Crmeh49);
            dbProvider.AddInParameter(command, helper.Crmeh48, DbType.Decimal, entity.Crmeh48);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceEnergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crmeh47, DbType.Decimal, entity.Crmeh47);
            dbProvider.AddInParameter(command, helper.Crmeh46, DbType.Decimal, entity.Crmeh46);
            dbProvider.AddInParameter(command, helper.Crmeh45, DbType.Decimal, entity.Crmeh45);
            dbProvider.AddInParameter(command, helper.Crmeh44, DbType.Decimal, entity.Crmeh44);
            dbProvider.AddInParameter(command, helper.Crmeh43, DbType.Decimal, entity.Crmeh43);
            dbProvider.AddInParameter(command, helper.Crmeh42, DbType.Decimal, entity.Crmeh42);
            dbProvider.AddInParameter(command, helper.Crmeh41, DbType.Decimal, entity.Crmeh41);
            dbProvider.AddInParameter(command, helper.Crmeh40, DbType.Decimal, entity.Crmeh40);
            dbProvider.AddInParameter(command, helper.Crmeh39, DbType.Decimal, entity.Crmeh39);
            dbProvider.AddInParameter(command, helper.Crmeh38, DbType.Decimal, entity.Crmeh38);
            dbProvider.AddInParameter(command, helper.Crmeh37, DbType.Decimal, entity.Crmeh37);
            dbProvider.AddInParameter(command, helper.Crmeh36, DbType.Decimal, entity.Crmeh36);
            dbProvider.AddInParameter(command, helper.Crmeh35, DbType.Decimal, entity.Crmeh35);
            dbProvider.AddInParameter(command, helper.Crmeh34, DbType.Decimal, entity.Crmeh34);
            dbProvider.AddInParameter(command, helper.Crmeh33, DbType.Decimal, entity.Crmeh33);
            dbProvider.AddInParameter(command, helper.Crmeh32, DbType.Decimal, entity.Crmeh32);
            dbProvider.AddInParameter(command, helper.Crmeh31, DbType.Decimal, entity.Crmeh31);
            dbProvider.AddInParameter(command, helper.Crmeh30, DbType.Decimal, entity.Crmeh30);
            dbProvider.AddInParameter(command, helper.Crmeh29, DbType.Decimal, entity.Crmeh29);
            dbProvider.AddInParameter(command, helper.Crmeh28, DbType.Decimal, entity.Crmeh28);
            dbProvider.AddInParameter(command, helper.Crmeh27, DbType.Decimal, entity.Crmeh27);
            dbProvider.AddInParameter(command, helper.Crmeh26, DbType.Decimal, entity.Crmeh26);
            dbProvider.AddInParameter(command, helper.Crmeh25, DbType.Decimal, entity.Crmeh25);
            dbProvider.AddInParameter(command, helper.Crmeh24, DbType.Decimal, entity.Crmeh24);
            dbProvider.AddInParameter(command, helper.Crmeh23, DbType.Decimal, entity.Crmeh23);
            dbProvider.AddInParameter(command, helper.Crmeh22, DbType.Decimal, entity.Crmeh22);
            dbProvider.AddInParameter(command, helper.Crmeh21, DbType.Decimal, entity.Crmeh21);
            dbProvider.AddInParameter(command, helper.Crmeh20, DbType.Decimal, entity.Crmeh20);
            dbProvider.AddInParameter(command, helper.Crmeh19, DbType.Decimal, entity.Crmeh19);
            dbProvider.AddInParameter(command, helper.Crmeh18, DbType.Decimal, entity.Crmeh18);
            dbProvider.AddInParameter(command, helper.Crmeh17, DbType.Decimal, entity.Crmeh17);
            dbProvider.AddInParameter(command, helper.Crmeh16, DbType.Decimal, entity.Crmeh16);
            dbProvider.AddInParameter(command, helper.Crmeh15, DbType.Decimal, entity.Crmeh15);
            dbProvider.AddInParameter(command, helper.Crmeh14, DbType.Decimal, entity.Crmeh14);
            dbProvider.AddInParameter(command, helper.Crmeh13, DbType.Decimal, entity.Crmeh13);
            dbProvider.AddInParameter(command, helper.Crmeh12, DbType.Decimal, entity.Crmeh12);
            dbProvider.AddInParameter(command, helper.Crmeh11, DbType.Decimal, entity.Crmeh11);
            dbProvider.AddInParameter(command, helper.Crmeh10, DbType.Decimal, entity.Crmeh10);
            dbProvider.AddInParameter(command, helper.Crmeh9, DbType.Decimal, entity.Crmeh9);
            dbProvider.AddInParameter(command, helper.Crmeh8, DbType.Decimal, entity.Crmeh8);
            dbProvider.AddInParameter(command, helper.Crmeh7, DbType.Decimal, entity.Crmeh7);
            dbProvider.AddInParameter(command, helper.Crmeh6, DbType.Decimal, entity.Crmeh6);
            dbProvider.AddInParameter(command, helper.Crmeh5, DbType.Decimal, entity.Crmeh5);
            dbProvider.AddInParameter(command, helper.Crmeh4, DbType.Decimal, entity.Crmeh4);
            dbProvider.AddInParameter(command, helper.Crmeh3, DbType.Decimal, entity.Crmeh3);
            dbProvider.AddInParameter(command, helper.Crmeh2, DbType.Decimal, entity.Crmeh2);
            dbProvider.AddInParameter(command, helper.Crmeh1, DbType.Decimal, entity.Crmeh1);
            dbProvider.AddInParameter(command, helper.Crmemeditotal, DbType.Decimal, entity.Crmemeditotal);
            dbProvider.AddInParameter(command, helper.Crmeh96, DbType.Decimal, entity.Crmeh96);
            dbProvider.AddInParameter(command, helper.Crmeh95, DbType.Decimal, entity.Crmeh95);
            dbProvider.AddInParameter(command, helper.Crmeh94, DbType.Decimal, entity.Crmeh94);
            dbProvider.AddInParameter(command, helper.Crmeh93, DbType.Decimal, entity.Crmeh93);
            dbProvider.AddInParameter(command, helper.Crmeh92, DbType.Decimal, entity.Crmeh92);
            dbProvider.AddInParameter(command, helper.Crmeh91, DbType.Decimal, entity.Crmeh91);
            dbProvider.AddInParameter(command, helper.Crmeh90, DbType.Decimal, entity.Crmeh90);
            dbProvider.AddInParameter(command, helper.Crmeh89, DbType.Decimal, entity.Crmeh89);
            dbProvider.AddInParameter(command, helper.Crmeh88, DbType.Decimal, entity.Crmeh88);
            dbProvider.AddInParameter(command, helper.Crmeh87, DbType.Decimal, entity.Crmeh87);
            dbProvider.AddInParameter(command, helper.Crmeh86, DbType.Decimal, entity.Crmeh86);
            dbProvider.AddInParameter(command, helper.Crmeh85, DbType.Decimal, entity.Crmeh85);
            dbProvider.AddInParameter(command, helper.Crmeh84, DbType.Decimal, entity.Crmeh84);
            dbProvider.AddInParameter(command, helper.Crmeh83, DbType.Decimal, entity.Crmeh83);
            dbProvider.AddInParameter(command, helper.Crmeh82, DbType.Decimal, entity.Crmeh82);
            dbProvider.AddInParameter(command, helper.Crmeh81, DbType.Decimal, entity.Crmeh81);
            dbProvider.AddInParameter(command, helper.Crmeh80, DbType.Decimal, entity.Crmeh80);
            dbProvider.AddInParameter(command, helper.Crmeh79, DbType.Decimal, entity.Crmeh79);
            dbProvider.AddInParameter(command, helper.Crmeh78, DbType.Decimal, entity.Crmeh78);
            dbProvider.AddInParameter(command, helper.Crmeh77, DbType.Decimal, entity.Crmeh77);
            dbProvider.AddInParameter(command, helper.Crmeh76, DbType.Decimal, entity.Crmeh76);
            dbProvider.AddInParameter(command, helper.Crmeh75, DbType.Decimal, entity.Crmeh75);
            dbProvider.AddInParameter(command, helper.Crmeh74, DbType.Decimal, entity.Crmeh74);
            dbProvider.AddInParameter(command, helper.Crmeh73, DbType.Decimal, entity.Crmeh73);
            dbProvider.AddInParameter(command, helper.Crmeh72, DbType.Decimal, entity.Crmeh72);
            dbProvider.AddInParameter(command, helper.Crmeh71, DbType.Decimal, entity.Crmeh71);
            dbProvider.AddInParameter(command, helper.Crmeh70, DbType.Decimal, entity.Crmeh70);
            dbProvider.AddInParameter(command, helper.Crmeh69, DbType.Decimal, entity.Crmeh69);
            dbProvider.AddInParameter(command, helper.Crmeh68, DbType.Decimal, entity.Crmeh68);
            dbProvider.AddInParameter(command, helper.Crmeh67, DbType.Decimal, entity.Crmeh67);
            dbProvider.AddInParameter(command, helper.Crmeh66, DbType.Decimal, entity.Crmeh66);
            dbProvider.AddInParameter(command, helper.Crmeh65, DbType.Decimal, entity.Crmeh65);
            dbProvider.AddInParameter(command, helper.Crmeh64, DbType.Decimal, entity.Crmeh64);
            dbProvider.AddInParameter(command, helper.Crmeh63, DbType.Decimal, entity.Crmeh63);
            dbProvider.AddInParameter(command, helper.Crmeh62, DbType.Decimal, entity.Crmeh62);
            dbProvider.AddInParameter(command, helper.Crmeh61, DbType.Decimal, entity.Crmeh61);
            dbProvider.AddInParameter(command, helper.Crmeh60, DbType.Decimal, entity.Crmeh60);
            dbProvider.AddInParameter(command, helper.Crmeh59, DbType.Decimal, entity.Crmeh59);
            dbProvider.AddInParameter(command, helper.Crmeh58, DbType.Decimal, entity.Crmeh58);
            dbProvider.AddInParameter(command, helper.Crmeh57, DbType.Decimal, entity.Crmeh57);
            dbProvider.AddInParameter(command, helper.Crmeh56, DbType.Decimal, entity.Crmeh56);
            dbProvider.AddInParameter(command, helper.Crmeh55, DbType.Decimal, entity.Crmeh55);
            dbProvider.AddInParameter(command, helper.Crmeh54, DbType.Decimal, entity.Crmeh54);
            dbProvider.AddInParameter(command, helper.Crmeh53, DbType.Decimal, entity.Crmeh53);
            dbProvider.AddInParameter(command, helper.Crmeh52, DbType.Decimal, entity.Crmeh52);
            dbProvider.AddInParameter(command, helper.Crmeh51, DbType.Decimal, entity.Crmeh51);
            dbProvider.AddInParameter(command, helper.Crmeh50, DbType.Decimal, entity.Crmeh50);
            dbProvider.AddInParameter(command, helper.Crmeh49, DbType.Decimal, entity.Crmeh49);
            dbProvider.AddInParameter(command, helper.Crmeh48, DbType.Decimal, entity.Crmeh48);
            dbProvider.AddInParameter(command, helper.Crmefecha, DbType.DateTime, entity.Crmefecha);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime crmefecha, int ptomedicodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crmefecha, DbType.DateTime, crmefecha);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceEnergiaDTO GetById(DateTime crmefecha, int ptomedicodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crmefecha, DbType.DateTime, crmefecha);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            VceEnergiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceEnergiaDTO> List()
        {
            List<VceEnergiaDTO> entitys = new List<VceEnergiaDTO>();
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

        public List<VceEnergiaDTO> GetByCriteria()
        {
            List<VceEnergiaDTO> entitys = new List<VceEnergiaDTO>();
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

        //METODO NETC
        public void SaveFromMeMedicion96(int pecacodi, string fechaini, string fechafin)
        {
            string queryString = string.Format(helper.SqlSaveFromMeMedicion96, pecacodi, fechaini, fechafin);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletexFecha(string fechaini, string fechafin)
        {
            string queryString = string.Format(helper.SqlDeletexFecha, fechaini, fechafin);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }


        public List<VceEnergiaDTO> ListByCriteria(int reg, int pecacodi)
        {   
            List<VceEnergiaDTO> entitys = new List<VceEnergiaDTO>();
            string queryString = string.Format(helper.SqlListByCriteria, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int x = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    if (x > reg)
                    {
                        return entitys;
                    }
                    x++;
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersion(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteByVersion, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
      
    }

}

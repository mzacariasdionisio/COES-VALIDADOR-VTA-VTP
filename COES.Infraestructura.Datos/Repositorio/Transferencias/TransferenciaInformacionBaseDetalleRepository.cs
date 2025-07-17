using COES.Base.Core;
using COES.Dominio.DTO;
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
    public class TransferenciaInformacionBaseDetalleRepository : RepositoryBase, ITransferenciaInformacionBaseDetalleRepository
    {
        public TransferenciaInformacionBaseDetalleRepository(string strConn)
            : base(strConn)
        {
        }

        TransferenciaInformacionBaseDetalleHelper helper = new TransferenciaInformacionBaseDetalleHelper();

        public int Save(Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.TINFBCODI, DbType.Int32, entity.TinfbCodi);
            dbProvider.AddInParameter(command, helper.TINFBDECODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.TINTFBDEVERSION, DbType.Int32, entity.TinfbDeVersion);
            dbProvider.AddInParameter(command, helper.TINFBDEDIA, DbType.Int32, entity.TinfbDeDia);
            dbProvider.AddInParameter(command, helper.TINFBDEPROMEDIODIA, DbType.Decimal, entity.TinfbDePromedioDia);
            dbProvider.AddInParameter(command, helper.TINFBDESUMADIA, DbType.Decimal, entity.TinfbDeSumaDia);
            dbProvider.AddInParameter(command, helper.TINFBDE1, DbType.Double, entity.TinfbDe1);
            dbProvider.AddInParameter(command, helper.TINFBDE2, DbType.Double, entity.TinfbDe2);
            dbProvider.AddInParameter(command, helper.TINFBDE3, DbType.Double, entity.TinfbDe3);
            dbProvider.AddInParameter(command, helper.TINFBDE4, DbType.Double, entity.TinfbDe4);
            dbProvider.AddInParameter(command, helper.TINFBDE5, DbType.Double, entity.TinfbDe5);
            dbProvider.AddInParameter(command, helper.TINFBDE6, DbType.Double, entity.TinfbDe6);
            dbProvider.AddInParameter(command, helper.TINFBDE7, DbType.Double, entity.TinfbDe7);
            dbProvider.AddInParameter(command, helper.TINFBDE8, DbType.Double, entity.TinfbDe8);
            dbProvider.AddInParameter(command, helper.TINFBDE9, DbType.Double, entity.TinfbDe9);
            dbProvider.AddInParameter(command, helper.TINFBDE10, DbType.Double, entity.TinfbDe10);
            dbProvider.AddInParameter(command, helper.TINFBDE11, DbType.Double, entity.TinfbDe11);
            dbProvider.AddInParameter(command, helper.TINFBDE12, DbType.Double, entity.TinfbDe12);
            dbProvider.AddInParameter(command, helper.TINFBDE13, DbType.Double, entity.TinfbDe13);
            dbProvider.AddInParameter(command, helper.TINFBDE14, DbType.Double, entity.TinfbDe14);
            dbProvider.AddInParameter(command, helper.TINFBDE15, DbType.Double, entity.TinfbDe15);
            dbProvider.AddInParameter(command, helper.TINFBDE16, DbType.Double, entity.TinfbDe16);
            dbProvider.AddInParameter(command, helper.TINFBDE17, DbType.Double, entity.TinfbDe17);
            dbProvider.AddInParameter(command, helper.TINFBDE18, DbType.Double, entity.TinfbDe18);
            dbProvider.AddInParameter(command, helper.TINFBDE19, DbType.Double, entity.TinfbDe19);
            dbProvider.AddInParameter(command, helper.TINFBDE20, DbType.Double, entity.TinfbDe20);
            dbProvider.AddInParameter(command, helper.TINFBDE21, DbType.Double, entity.TinfbDe21);
            dbProvider.AddInParameter(command, helper.TINFBDE22, DbType.Double, entity.TinfbDe22);
            dbProvider.AddInParameter(command, helper.TINFBDE23, DbType.Double, entity.TinfbDe23);
            dbProvider.AddInParameter(command, helper.TINFBDE24, DbType.Double, entity.TinfbDe24);
            dbProvider.AddInParameter(command, helper.TINFBDE25, DbType.Double, entity.TinfbDe25);
            dbProvider.AddInParameter(command, helper.TINFBDE26, DbType.Double, entity.TinfbDe26);
            dbProvider.AddInParameter(command, helper.TINFBDE27, DbType.Double, entity.TinfbDe27);
            dbProvider.AddInParameter(command, helper.TINFBDE28, DbType.Double, entity.TinfbDe28);
            dbProvider.AddInParameter(command, helper.TINFBDE29, DbType.Double, entity.TinfbDe29);
            dbProvider.AddInParameter(command, helper.TINFBDE30, DbType.Double, entity.TinfbDe30);
            dbProvider.AddInParameter(command, helper.TINFBDE31, DbType.Double, entity.TinfbDe31);
            dbProvider.AddInParameter(command, helper.TINFBDE32, DbType.Double, entity.TinfbDe32);
            dbProvider.AddInParameter(command, helper.TINFBDE33, DbType.Double, entity.TinfbDe33);
            dbProvider.AddInParameter(command, helper.TINFBDE34, DbType.Double, entity.TinfbDe34);
            dbProvider.AddInParameter(command, helper.TINFBDE35, DbType.Double, entity.TinfbDe35);
            dbProvider.AddInParameter(command, helper.TINFBDE36, DbType.Double, entity.TinfbDe36);
            dbProvider.AddInParameter(command, helper.TINFBDE37, DbType.Double, entity.TinfbDe37);
            dbProvider.AddInParameter(command, helper.TINFBDE38, DbType.Double, entity.TinfbDe38);
            dbProvider.AddInParameter(command, helper.TINFBDE39, DbType.Double, entity.TinfbDe39);
            dbProvider.AddInParameter(command, helper.TINFBDE40, DbType.Double, entity.TinfbDe40);
            dbProvider.AddInParameter(command, helper.TINFBDE41, DbType.Double, entity.TinfbDe41);
            dbProvider.AddInParameter(command, helper.TINFBDE42, DbType.Double, entity.TinfbDe42);
            dbProvider.AddInParameter(command, helper.TINFBDE43, DbType.Double, entity.TinfbDe43);
            dbProvider.AddInParameter(command, helper.TINFBDE44, DbType.Double, entity.TinfbDe44);
            dbProvider.AddInParameter(command, helper.TINFBDE45, DbType.Double, entity.TinfbDe45);
            dbProvider.AddInParameter(command, helper.TINFBDE46, DbType.Double, entity.TinfbDe46);
            dbProvider.AddInParameter(command, helper.TINFBDE47, DbType.Double, entity.TinfbDe47);
            dbProvider.AddInParameter(command, helper.TINFBDE48, DbType.Double, entity.TinfbDe48);
            dbProvider.AddInParameter(command, helper.TINFBDE49, DbType.Double, entity.TinfbDe49);
            dbProvider.AddInParameter(command, helper.TINFBDE50, DbType.Double, entity.TinfbDe50);
            dbProvider.AddInParameter(command, helper.TINFBDE51, DbType.Double, entity.TinfbDe51);
            dbProvider.AddInParameter(command, helper.TINFBDE52, DbType.Double, entity.TinfbDe52);
            dbProvider.AddInParameter(command, helper.TINFBDE53, DbType.Double, entity.TinfbDe53);
            dbProvider.AddInParameter(command, helper.TINFBDE54, DbType.Double, entity.TinfbDe54);
            dbProvider.AddInParameter(command, helper.TINFBDE55, DbType.Double, entity.TinfbDe55);
            dbProvider.AddInParameter(command, helper.TINFBDE56, DbType.Double, entity.TinfbDe56);
            dbProvider.AddInParameter(command, helper.TINFBDE57, DbType.Double, entity.TinfbDe57);
            dbProvider.AddInParameter(command, helper.TINFBDE58, DbType.Double, entity.TinfbDe58);
            dbProvider.AddInParameter(command, helper.TINFBDE59, DbType.Double, entity.TinfbDe59);
            dbProvider.AddInParameter(command, helper.TINFBDE60, DbType.Double, entity.TinfbDe60);
            dbProvider.AddInParameter(command, helper.TINFBDE61, DbType.Double, entity.TinfbDe61);
            dbProvider.AddInParameter(command, helper.TINFBDE62, DbType.Double, entity.TinfbDe62);
            dbProvider.AddInParameter(command, helper.TINFBDE63, DbType.Double, entity.TinfbDe63);
            dbProvider.AddInParameter(command, helper.TINFBDE64, DbType.Double, entity.TinfbDe64);
            dbProvider.AddInParameter(command, helper.TINFBDE65, DbType.Double, entity.TinfbDe65);
            dbProvider.AddInParameter(command, helper.TINFBDE66, DbType.Double, entity.TinfbDe66);
            dbProvider.AddInParameter(command, helper.TINFBDE67, DbType.Double, entity.TinfbDe67);
            dbProvider.AddInParameter(command, helper.TINFBDE68, DbType.Double, entity.TinfbDe68);
            dbProvider.AddInParameter(command, helper.TINFBDE69, DbType.Double, entity.TinfbDe69);
            dbProvider.AddInParameter(command, helper.TINFBDE70, DbType.Double, entity.TinfbDe70);
            dbProvider.AddInParameter(command, helper.TINFBDE71, DbType.Double, entity.TinfbDe71);
            dbProvider.AddInParameter(command, helper.TINFBDE72, DbType.Double, entity.TinfbDe72);
            dbProvider.AddInParameter(command, helper.TINFBDE73, DbType.Double, entity.TinfbDe73);
            dbProvider.AddInParameter(command, helper.TINFBDE74, DbType.Double, entity.TinfbDe74);
            dbProvider.AddInParameter(command, helper.TINFBDE75, DbType.Double, entity.TinfbDe75);
            dbProvider.AddInParameter(command, helper.TINFBDE76, DbType.Double, entity.TinfbDe76);
            dbProvider.AddInParameter(command, helper.TINFBDE77, DbType.Double, entity.TinfbDe77);
            dbProvider.AddInParameter(command, helper.TINFBDE78, DbType.Double, entity.TinfbDe78);
            dbProvider.AddInParameter(command, helper.TINFBDE79, DbType.Double, entity.TinfbDe79);
            dbProvider.AddInParameter(command, helper.TINFBDE80, DbType.Double, entity.TinfbDe80);
            dbProvider.AddInParameter(command, helper.TINFBDE81, DbType.Double, entity.TinfbDe81);
            dbProvider.AddInParameter(command, helper.TINFBDE82, DbType.Double, entity.TinfbDe82);
            dbProvider.AddInParameter(command, helper.TINFBDE83, DbType.Double, entity.TinfbDe83);
            dbProvider.AddInParameter(command, helper.TINFBDE84, DbType.Double, entity.TinfbDe84);
            dbProvider.AddInParameter(command, helper.TINFBDE85, DbType.Double, entity.TinfbDe85);
            dbProvider.AddInParameter(command, helper.TINFBDE86, DbType.Double, entity.TinfbDe86);
            dbProvider.AddInParameter(command, helper.TINFBDE87, DbType.Double, entity.TinfbDe87);
            dbProvider.AddInParameter(command, helper.TINFBDE88, DbType.Double, entity.TinfbDe88);
            dbProvider.AddInParameter(command, helper.TINFBDE89, DbType.Double, entity.TinfbDe89);
            dbProvider.AddInParameter(command, helper.TINFBDE90, DbType.Double, entity.TinfbDe90);
            dbProvider.AddInParameter(command, helper.TINFBDE91, DbType.Double, entity.TinfbDe91);
            dbProvider.AddInParameter(command, helper.TINFBDE92, DbType.Double, entity.TinfbDe92);
            dbProvider.AddInParameter(command, helper.TINFBDE93, DbType.Double, entity.TinfbDe93);
            dbProvider.AddInParameter(command, helper.TINFBDE94, DbType.Double, entity.TinfbDe94);
            dbProvider.AddInParameter(command, helper.TINFBDE95, DbType.Double, entity.TinfbDe95);
            dbProvider.AddInParameter(command, helper.TINFBDE96, DbType.Double, entity.TinfbDe96);
            dbProvider.AddInParameter(command, helper.TINFBDEUSERNAME, DbType.String, entity.TinfbDeUserName);
            dbProvider.AddInParameter(command, helper.TINFBDEFECINS, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int pericodi, int version, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TINTFBDEVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, sCodigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaTransferenciaInfoBaseDetalle(int iPeriCodi, int iVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaInfoBaseDetalle);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TINTFBDEVERSION, DbType.Int32, iVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TINFBDECODI, DbType.Int32, id);
            TransferenciaInformacionBaseDetalleDTO entity = null;

            return entity;
        }

        public List<Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO> List(int emprcodi, int pericodi)
        {
            List<TransferenciaInformacionBaseDetalleDTO> entitys = new List<TransferenciaInformacionBaseDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaInformacionBaseDetalleDTO entity = new TransferenciaInformacionBaseDetalleDTO();

                    int ITINFBCODIGO = dr.GetOrdinal(helper.TINFBCODIGO);
                    if (!dr.IsDBNull(ITINFBCODIGO)) entity.TinfbCodigo = dr.GetString(ITINFBCODIGO);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO> GetByCriteria(int emprcodi, int pericodi, string solicodireticodigo, int version)
        {
            List<TransferenciaInformacionBaseDetalleDTO> entitys = new List<TransferenciaInformacionBaseDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, solicodireticodigo);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, solicodireticodigo);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));

                }
            }

            return entitys;
        }


        public List<Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO> GetByPeriodoVersion(int pericodi, int version)
        {
              List<TransferenciaInformacionBaseDetalleDTO> entitys = new List<TransferenciaInformacionBaseDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaInformacionBaseDetalleDTO entity = new TransferenciaInformacionBaseDetalleDTO();

                    int iEMPRCODI = dr.GetOrdinal(this.helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    int iBARRCODI = dr.GetOrdinal(this.helper.BARRCODI);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int ITINFBCODIGO = dr.GetOrdinal(this.helper.TINFBCODIGO);
                    if (!dr.IsDBNull(ITINFBCODIGO)) entity.TinfbCodigo = dr.GetString(ITINFBCODIGO);

                    int IEQUICODI = dr.GetOrdinal(this.helper.EQUICODI);
                    if (!dr.IsDBNull(IEQUICODI)) entity.CentGeneCodi = dr.GetInt32(IEQUICODI);

                    int iPERICODI = dr.GetOrdinal(this.helper.PERICODI);
                    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

                    int iTINFBVERSION = dr.GetOrdinal(this.helper.TINFBVERSION);
                    if (!dr.IsDBNull(iTINFBVERSION)) entity.TinfbVersion = dr.GetInt32(iTINFBVERSION);

                    int iTINFBTIPOINFORMACION = dr.GetOrdinal(this.helper.TINFBTIPOINFORMACION);
                    if (!dr.IsDBNull(iTINFBTIPOINFORMACION)) entity.TinfbTipoInformacion = dr.GetString(iTINFBTIPOINFORMACION);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }


        public List<Dominio.DTO.Transferencias.TransferenciaInformacionBaseDetalleDTO> ListByTransferenciaInfoBase(int iTInfbCodi)
        {
            List<TransferenciaInformacionBaseDetalleDTO> entitys = new List<TransferenciaInformacionBaseDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByTransferenciaInfoBase);

            dbProvider.AddInParameter(command, helper.TINFBCODI, DbType.Int32, iTInfbCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);

            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
    }
}

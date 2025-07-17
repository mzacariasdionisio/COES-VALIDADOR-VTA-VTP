using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PmoDatPmhiTrDTO : EntityBase
    {
        public int PmPmhtCodi { get; set; }
        public int Sddpcodi { get; set; }
        public int PmPeriCodi { get; set; }
        public int Sddpnum { get; set; }
        public string Sddpnomb { get; set; }

        public int PmPmhtAnhio { get; set; }
        public decimal? PmPmhtDisp01 { get; set; }
        public decimal? PmPmhtDisp02 { get; set; }
        public decimal? PmPmhtDisp03 { get; set; }
        public decimal? PmPmhtDisp04 { get; set; }
        public decimal? PmPmhtDisp05 { get; set; }
        public decimal? PmPmhtDisp06 { get; set; }
        public decimal? PmPmhtDisp07 { get; set; }
        public decimal? PmPmhtDisp08 { get; set; }
        public decimal? PmPmhtDisp09 { get; set; }
        public decimal? PmPmhtDisp10 { get; set; }
        public decimal? PmPmhtDisp11 { get; set; }
        public decimal? PmPmhtDisp12 { get; set; }
        public decimal? PmPmhtDisp13 { get; set; }
        public decimal? PmPmhtDisp14 { get; set; }
        public decimal? PmPmhtDisp15 { get; set; }
        public decimal? PmPmhtDisp16 { get; set; }
        public decimal? PmPmhtDisp17 { get; set; }
        public decimal? PmPmhtDisp18 { get; set; }
        public decimal? PmPmhtDisp19 { get; set; }
        public decimal? PmPmhtDisp20 { get; set; }
        public decimal? PmPmhtDisp21 { get; set; }
        public decimal? PmPmhtDisp22 { get; set; }
        public decimal? PmPmhtDisp23 { get; set; }
        public decimal? PmPmhtDisp24 { get; set; }
        public decimal? PmPmhtDisp25 { get; set; }
        public decimal? PmPmhtDisp26 { get; set; }
        public decimal? PmPmhtDisp27 { get; set; }
        public decimal? PmPmhtDisp28 { get; set; }
        public decimal? PmPmhtDisp29 { get; set; }
        public decimal? PmPmhtDisp30 { get; set; }
        public decimal? PmPmhtDisp31 { get; set; }
        public decimal? PmPmhtDisp32 { get; set; }
        public decimal? PmPmhtDisp33 { get; set; }
        public decimal? PmPmhtDisp34 { get; set; }
        public decimal? PmPmhtDisp35 { get; set; }
        public decimal? PmPmhtDisp36 { get; set; }
        public decimal? PmPmhtDisp37 { get; set; }
        public decimal? PmPmhtDisp38 { get; set; }
        public decimal? PmPmhtDisp39 { get; set; }
        public decimal? PmPmhtDisp40 { get; set; }
        public decimal? PmPmhtDisp41 { get; set; }
        public decimal? PmPmhtDisp42 { get; set; }
        public decimal? PmPmhtDisp43 { get; set; }
        public decimal? PmPmhtDisp44 { get; set; }
        public decimal? PmPmhtDisp45 { get; set; }
        public decimal? PmPmhtDisp46 { get; set; }
        public decimal? PmPmhtDisp47 { get; set; }
        public decimal? PmPmhtDisp48 { get; set; }
        public decimal? PmPmhtDisp49 { get; set; }
        public decimal? PmPmhtDisp50 { get; set; }
        public decimal? PmPmhtDisp51 { get; set; }
        public decimal? PmPmhtDisp52 { get; set; }
        public decimal? PmPmhtDisp53 { get; set; }
        public string PmPmhtTipo { get; set; }

        public string Planta { get; set; }
        #region 20190308 - NET: Adecuaciones a los archivos .DAT
        public string strPmPmhtDisp01 
        {
            get {
                if (PmPmhtDisp01.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp01.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp01);                    
                }
            
        }
        public string strPmPmhtDisp02
        {
            get
            {
                if (PmPmhtDisp02.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp02.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp02);
                    
            }

        }
        public string strPmPmhtDisp03
        {
            get
            {
                if (PmPmhtDisp03.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp03.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp03);                    
                        
            }

        }
        public string strPmPmhtDisp04
        {
            get
            {
                if (PmPmhtDisp04.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp04.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp04);

            }

        }
        public string strPmPmhtDisp05
        {
            get
            {
                if (PmPmhtDisp05.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp05.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp05);

            }

        }
        public string strPmPmhtDisp06
        {
            get
            {
                if (PmPmhtDisp06.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp06.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp06);

            }

        }
        public string strPmPmhtDisp07
        {
            get
            {
                if (PmPmhtDisp07.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp07.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp07);

            }

        }
        public string strPmPmhtDisp08
        {
            get
            {
                if (PmPmhtDisp08.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp08.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp08);

            }

        }
        public string strPmPmhtDisp09
        {
            get
            {
                if (PmPmhtDisp09.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp09.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp09);

            }

        }
        public string strPmPmhtDisp10
        {
            get
            {
                if (PmPmhtDisp10.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp10.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp10);

            }

        }
        public string strPmPmhtDisp11
        {
            get
            {
                if (PmPmhtDisp11.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp11.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp11);

            }

        }
        public string strPmPmhtDisp12
        {
            get
            {
                if (PmPmhtDisp12.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp12.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp12);

            }

        }
        public string strPmPmhtDisp13
        {
            get
            {
                if (PmPmhtDisp13.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp13.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp13);

            }

        }
        public string strPmPmhtDisp14
        {
            get
            {
                if (PmPmhtDisp14.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp14.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp14);

            }

        }
        public string strPmPmhtDisp15
        {
            get
            {
                if (PmPmhtDisp15.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp15.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp15);

            }

        }
        public string strPmPmhtDisp16
        {
            get
            {
                if (PmPmhtDisp16.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp16.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp16);

            }

        }
        public string strPmPmhtDisp17
        {
            get
            {
                if (PmPmhtDisp17.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp17.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp17);

            }

        }
        public string strPmPmhtDisp18
        {
            get
            {
                if (PmPmhtDisp18.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp18.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp18);

            }

        }
        public string strPmPmhtDisp19
        {
            get
            {
                if (PmPmhtDisp19.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp19.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp19);

            }

        }
        public string strPmPmhtDisp20
        {
            get
            {
                if (PmPmhtDisp20.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp20.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp20);

            }

        }
        public string strPmPmhtDisp21
        {
            get
            {
                if (PmPmhtDisp21.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp21.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp21);

            }

        }
        public string strPmPmhtDisp22
        {
            get
            {
                if (PmPmhtDisp22.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp22.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp22);

            }

        }
        public string strPmPmhtDisp23
        {
            get
            {
                if (PmPmhtDisp23.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp23.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp23);

            }

        }
        public string strPmPmhtDisp24
        {
            get
            {
                if (PmPmhtDisp24.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp24.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp24);

            }

        }
        public string strPmPmhtDisp25
        {
            get
            {
                if (PmPmhtDisp25.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp25.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp25);

            }

        }
        public string strPmPmhtDisp26
        {
            get
            {
                if (PmPmhtDisp26.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp26.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp26);

            }

        }
        public string strPmPmhtDisp27
        {
            get
            {
                if (PmPmhtDisp27.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp27.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp27);

            }

        }
        public string strPmPmhtDisp28
        {
            get
            {
                if (PmPmhtDisp28.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp28.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp28);

            }

        }
        public string strPmPmhtDisp29
        {
            get
            {
                if (PmPmhtDisp29.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp29.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp29);

            }

        }
        public string strPmPmhtDisp30
        {
            get
            {
                if (PmPmhtDisp30.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp30.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp30);

            }

        }
        public string strPmPmhtDisp31
        {
            get
            {
                if (PmPmhtDisp31.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp31.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp31);

            }

        }
        public string strPmPmhtDisp32
        {
            get
            {
                if (PmPmhtDisp32.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp32.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp32);

            }

        }
        public string strPmPmhtDisp33
        {
            get
            {
                if (PmPmhtDisp33.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp33.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp33);

            }

        }
        public string strPmPmhtDisp34
        {
            get
            {
                if (PmPmhtDisp34.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp34.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp34);

            }

        }
        public string strPmPmhtDisp35
        {
            get
            {
                if (PmPmhtDisp35.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp35.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp35);

            }

        }
        public string strPmPmhtDisp36
        {
            get
            {
                if (PmPmhtDisp36.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp36.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp36);

            }

        }
        public string strPmPmhtDisp37
        {
            get
            {
                if (PmPmhtDisp37.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp37.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp37);

            }

        }
        public string strPmPmhtDisp38
        {
            get
            {
                if (PmPmhtDisp38.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp38.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp38);

            }

        }
        public string strPmPmhtDisp39
        {
            get
            {
                if (PmPmhtDisp39.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp39.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp39);

            }

        }
        public string strPmPmhtDisp40
        {
            get
            {
                if (PmPmhtDisp40.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp40.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp40);

            }

        }
        public string strPmPmhtDisp41
        {
            get
            {
                if (PmPmhtDisp41.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp41.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp41);

            }

        }
        public string strPmPmhtDisp42
        {
            get
            {
                if (PmPmhtDisp42.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp42.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp42);

            }

        }
        public string strPmPmhtDisp43
        {
            get
            {
                if (PmPmhtDisp43.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp43.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp43);

            }

        }
        public string strPmPmhtDisp44
        {
            get
            {
                if (PmPmhtDisp44.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp44.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp44);

            }

        }
        public string strPmPmhtDisp45
        {
            get
            {
                if (PmPmhtDisp45.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp45.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp45);

            }

        }
        public string strPmPmhtDisp46
        {
            get
            {
                if (PmPmhtDisp46.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp46.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp46);

            }

        }
        public string strPmPmhtDisp47
        {
            get
            {
                if (PmPmhtDisp47.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp47.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp47);

            }

        }
        public string strPmPmhtDisp48
        {
            get
            {
                if (PmPmhtDisp48.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp48.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp48);

            }

        }
        public string strPmPmhtDisp49
        {
            get
            {
                if (PmPmhtDisp49.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp49.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp49);

            }

        }
        public string strPmPmhtDisp50
        {
            get
            {
                if (PmPmhtDisp50.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp50.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp50);

            }

        }
        public string strPmPmhtDisp51
        {
            get
            {
                if (PmPmhtDisp51.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp51.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp51);

            }

        }
        public string strPmPmhtDisp52
        {
            get
            {
                if (PmPmhtDisp52.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp52.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp52);

            }

        }
        public string strPmPmhtDisp53
        {
            get
            {
                if (PmPmhtDisp53.ToString().IndexOf('.') == -1)
                    return PmPmhtDisp53.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmPmhtDisp53);

            }

        }

        #endregion


    }
}

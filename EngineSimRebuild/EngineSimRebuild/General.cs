using System;
using System.Drawing;
using System.Windows.Forms;

namespace EngineSimRebuild
{
    public partial class General : Form
    {
        public General()
        {
            InitializeComponent();
            // Это оригинальная программа EngineSimU, которую я перенес (НА СКОРУЮ РУКУ) с Java на C# и адаптировал под славян
            // программа расчитана на расчет ТРД, ТРДФ и ТРДД. У меня были сжатые сроки, так что не надо плакать, что я насрал тут))

            // vk.com/65k_na_moih_nogah

            this.Text = "EngineSimU Rebuild";
            this.StartPosition = FormStartPosition.CenterScreen;

            setDefaultsNASA(); // начальные значения NASA EngineSimU
        }

        #region Vars
        private bool hideTotalParameters = true;

        private int iter = 0;
        private int OX;
        private int OY;
        private int iX = 0;
        private const int stageLength = 14;
        private int inletLength = 5 * stageLength;
        private int burnerLength = 5 * stageLength;
        private int nozzleLength = 4 * stageLength;
        private int afterBurnerLength = 8 * stageLength;
        private int fanLength = 5 * stageLength;
        private int turbineLength = (stageLength + 4) * (nturb + 3);
        private int fullLength = 1;
        private int compLength = 1;
        // Кисти
        private SolidBrush brush = new SolidBrush(Color.White); // Для текста
        private SolidBrush brushInlet = new SolidBrush(Color.Khaki); // Для ВУ
        private SolidBrush brushFan = new SolidBrush(Color.GreenYellow); // Для вентилятора
        private SolidBrush brushComp = new SolidBrush(Color.Cyan); // Для компрессора
        private SolidBrush brushBurner = new SolidBrush(Color.Firebrick); // Для КС
        private SolidBrush brushTurbine = new SolidBrush(Color.MediumOrchid); // Для турбины
        private SolidBrush brushNozzle = new SolidBrush(Color.Peru); // Для сопла

        // Reverse to METRIC
        private const double lbs_N = 4.4482;
        private const double lbps_kgps = 1.0 / 2.20462;
        private const double forSFC = 1.0 / 9.945455;
        private const double forHu = 1.0 / 0.429923;
        private const double mph_kmh = 1.60934;
        private const double ft_km = 0.0003048;
        private const double R_K = 0.555556;
        private const double lb_sq_inch__kPa = 6.89476;
        private const double ft_m = 0.3048;

        private double MOFCarbon;
        private double MOFHydrogen;
        private double MOFOxygen;

        private int abflag, entype, lunits, inflag, varflag, pt2flag, wtflag;
        private int abkeep, pltkeep, move;
        private int numeng, gamopt, arsched, plttyp, showcom;
        private int athsched, aexsched, fueltype, inptype, siztype;
        // Flow variables
        static double g0d, g0, rgas, gama, cpair;
        static double tt4, tt4d, tt7, tt7d, t8, p3p2d, p3fp2d, byprat, throtl;
        static double fsmach, altd, alt, ts0, ps0, q0, u0d, u0, a0, rho0, tsout, psout;
        static double epr, etr, npr, snpr, fnet, fgros, dram, sfc, fa, eair, uexit, ues;
        static double fnd, fnlb, fglb, drlb, flflo, fuelrat, fntot, eteng;
        static double arth, arthd, arexit, arexitd;
        static double mexit, pexit, pfexit;
        static double arthmn, arthmx, arexmn, arexmx;

        static double a8, a8rat, a8d, afan, a7, m2, isp;

        static double ac, a2, a2d, acore, a4, a4p, fhv, fhvd, mfr, diameng;

        static double altmin, altmax, u0min, u0max, thrmin, thrmax, pmax, tmin, tmax;
        static double u0mt, u0mr, altmt, altmr;
        static double etmin, etmax, cprmin, cprmax, t4min, t4max, pt4max;
        static double a2min, a2max, a8min, a8max, t7min, t7max, diamin, diamax;
        static double bypmin, bypmax, fprmin, fprmax;
        static double vmn1, vmn2, vmn3, vmn4, vmx1, vmx2, vmx3, vmx4;
        static double lconv1, lconv2, fconv, pconv, tconv, tref, mconv1, mconv2, econv, econv2;
        static double aconv, bconv, dconv, flconv;
        // weight and materials
        static double weight, wtref, wfref;
        static int mcomp, mfan, mturbin, mburner, minlt, mnozl, mnozr;
        static int ncflag, ncomp, ntflag, nturb, fireflag;
        static double dcomp, dfan, dturbin, dburner;
        static double tcomp, tfan, tturbin, tburner;
        static double tinlt, dinlt, tnozl, dnozl, tnozr, dnozr;
        static double lcomp, lburn, lturb, lnoz;   // component length
                                                   // Station Variables
        static double[] trat = new double[20];
        static double[] tt = new double[20];
        static double[] prat = new double[20];
        static double[] pt = new double[20];
        static double[] eta = new double[20];
        static double[] gam = new double[20];
        static double[] cp = new double[20];
        static double[] s = new double[20];
        static double[] v = new double[20];
        //  Percentage  variables
        static double u0ref, altref, thrref, a2ref, et2ref, fpref, et13ref, bpref;
        static double cpref, et3ref, et4ref, et5ref, t4ref, p4ref, t7ref, et7ref, a8ref;
        static double fnref, fuelref, sfcref, airref, epref, etref, faref;
        // save design
        int ensav, absav, gamosav, ptfsav, arssav, arthsav, arxsav, flsav;
        static double fhsav, t4sav, t7sav, p3sav, p3fsav, bysav, acsav;
        static double a2sav, a4sav, a4psav, gamsav, et2sav, pr2sav, pr4sav;
        static double et3sav, et4sav, et5sav, et7sav, et13sav, a8sav, a8mxsav;
        static double a8rtsav, u0mxsav, u0sav, altsav;
        static double trsav, artsav, arexsav;
        // save materials info
        int wtfsav, minsav, mfnsav, mcmsav, mbrsav, mtrsav, mnlsav, mnrsav, ncsav, ntsav;
        static double wtsav, dinsav, tinsav, dfnsav, tfnsav, dcmsav, tcmsav;
        static double dbrsav, tbrsav, dtrsav, ttrsav, dnlsav, tnlsav, dnrsav, tnrsav;
        // plot variables
        private int lines, nord, nabs, param, npt, ntikx, ntiky;
        private int counter;
        private int ordkeep, abskeep;
        static double begx, endx, begy, endy;
        static double[] pltx = new double[26];
        static double[] plty = new double[26];
        static String labx, laby, labyu, labxu;
        #endregion

        #region Get Data
        private void getData()
        {
            textBoxDiameter.Text = removeDots(textBoxDiameter.Text);
            diameng = Convert.ToDouble(textBoxDiameter.Text) / ft_m; // Диаметр
            if (diameng * ft_m > 2.43) diameng = 2.43 / ft_m;

            textBoxMachNumber.Text = removeDots(textBoxMachNumber.Text);
            fsmach = Convert.ToDouble(textBoxMachNumber.Text); // Число Маха
            if (fsmach > 2.25) diameng = 2.25;

            textBoxVFlight.Text = removeDots(textBoxVFlight.Text);
            u0d = Convert.ToDouble(textBoxVFlight.Text) / mph_kmh; // Скорость полета
            if (u0d > 2400.0 * mph_kmh) u0d = 2400.0 / mph_kmh;

            textBoxAltitude.Text = removeDots(textBoxAltitude.Text);
            altd = Convert.ToDouble(textBoxAltitude.Text) / ft_km; // Высота полета
            //if (altd > 18.1 * ft_km) altd = 18.1 / ft_km;

            textBoxT0.Text = removeDots(textBoxT0.Text);
            //ts0 = ((Convert.ToDouble(textBoxT0.Text) - 32.0) * 5.0 / 9.0) + 273.15; // Температура невозмущенного потока
            ts0 = Convert.ToDouble(textBoxT0.Text) / R_K; // Температура невозмущенного потока
            //if (ts0 < (200.0  - 32.0) * 5.0 / 9.0 + 273.15) ts0 = (200.0 - 32.0) * 5.0 / 9.0 + 273.15;

            textBoxP0.Text = removeDots(textBoxP0.Text);
            ps0 = Convert.ToDouble(textBoxP0.Text) / lb_sq_inch__kPa; // Давление невозмущенного потока

            textBoxDrossel.Text = removeDots(textBoxDrossel.Text);
            throtl = Convert.ToDouble(textBoxDrossel.Text); // РУД
            if (throtl > 100.0) throtl = 100.0;

            textBoxHu.Text = removeDots(textBoxHu.Text);
            fhv = Convert.ToDouble(textBoxHu.Text) / forHu; // Низшая теплота сгорания

            //eps = Convert.ToInt32(textBoxEps.Text); // Точноcть
            //if (eps > 7) eps = 7;

            textBoxSigmaInlet.Text = removeDots(textBoxSigmaInlet.Text);
            eta[2] = Convert.ToDouble(textBoxSigmaInlet.Text); // Коэффициент восстановления давления в ВУ
            if (eta[2] > 1.0) eta[2] = 1.0;
            if (eta[2] < 0.5) eta[2] = 0.5;

            textBoxPiFan.Text = removeDots(textBoxPiFan.Text);
            p3fp2d = Convert.ToDouble(textBoxPiFan.Text); // Степень повышения давления в вентиляторе
            if (p3fp2d > 2.0) p3fp2d = 2.0;
            if (p3fp2d < 1.0) p3fp2d = 1.0;

            textBox_m.Text = removeDots(textBox_m.Text);
            byprat = Convert.ToDouble(textBox_m.Text); // Степень двухконтурности
            if (byprat > 10.0) byprat = 10.0;
            if (byprat < 1.0) byprat = 1.0;

            textBoxEtaFan.Text = removeDots(textBoxEtaFan.Text);
            eta[13] = Convert.ToDouble(textBoxEtaFan.Text); // КПД вентилятора
            if (eta[13] > 1.0) eta[13] = 1.0;
            if (eta[13] < 0.5) eta[13] = 0.5;

            textBoxPiComp.Text = removeDots(textBoxPiComp.Text);
            prat[3] = p3p2d = Convert.ToDouble(textBoxPiComp.Text);// Степень повышения давления в компрессоре
            if (p3p2d > 50.0) prat[3] = p3p2d = 50.0;
            if (p3p2d < 1.0) prat[3] = p3p2d = 1.0;

            textBoxEtaComp.Text = removeDots(textBoxEtaComp.Text);
            eta[3] = Convert.ToDouble(textBoxEtaComp.Text); // КПД компрессора
            if (eta[3] > 1.0) eta[3] = 1.0;
            if (eta[3] < 0.5) eta[3] = 0.5;

            textBoxT4.Text = removeDots(textBoxT4.Text);
            tt[4] = tt4 = tt4d = Convert.ToDouble(textBoxT4.Text) / R_K; // Температура перед турбиной

            textBoxSigmaBurner.Text = removeDots(textBoxSigmaBurner.Text);
            prat[4] = Convert.ToDouble(textBoxSigmaBurner.Text); // Коэффициент восстановления давления в КС
            if (prat[4] > 1.0) prat[4] = 1.0;
            if (prat[4] < 0.5) prat[4] = 0.5;

            textBoxEtaBurner.Text = removeDots(textBoxEtaBurner.Text);
            eta[4] = Convert.ToDouble(textBoxEtaBurner.Text); // КПД сгорания
            if (eta[4] > 1.0) eta[4] = 1.0;
            if (eta[4] < 0.5) eta[4] = 0.5;

            textBoxEtaTurbine.Text = removeDots(textBoxEtaTurbine.Text);
            eta[5] = Convert.ToDouble(textBoxEtaTurbine.Text); // КПД турбины
            if (eta[5] > 1.0) eta[5] = 1.0;
            if (eta[5] < 0.5) eta[5] = 0.5;

            textBoxFiNozzle.Text = removeDots(textBoxFiNozzle.Text);
            eta[7] = Convert.ToDouble(textBoxFiNozzle.Text); // Коэффициент скорости сопла
            if (eta[7] > 1.0) eta[7] = 1.0;
            if (eta[7] < 0.5) eta[7] = 0.5;

            textBoxAfterBurner.Text = removeDots(textBoxAfterBurner.Text);
            tt[7] = tt7 = tt7d = Convert.ToDouble(textBoxAfterBurner.Text) / R_K; // Температура ФК
        }
        #endregion

        #region Compute
        private void comPute()
        {
            numeng = 1;
            fireflag = 0;
            getFuel();
            getFreeStream();
            getThermo();
            getGeo();
            getPerform();
        }
        #endregion

        #region Push Data
        private void pushData()
        {
            textBoxDiameter.Text = Convert.ToString(filter(diameng * ft_m)); // Диаметр
            textBoxMachNumber.Text = Convert.ToString(filter(fsmach)); // Число Маха
            textBoxVFlight.Text = Convert.ToString(filter(u0d * mph_kmh)); // Скорость полета
            textBoxAltitude.Text = Convert.ToString(filter(altd * ft_km)); // Высота полета
            textBoxT0.Text = Convert.ToString(filter(ts0 * R_K)); // Температура невозмущенного потока
            textBoxP0.Text = Convert.ToString(filter(ps0 * lb_sq_inch__kPa)); // Давление невозмущенного потока
            textBoxDrossel.Text = Convert.ToString(filter(throtl)); // РУД

            textBoxHu.Text = Convert.ToString(filter(fhv * forHu)); // Низшая теплота сгорания
            textBoxMOF_C.Text = Convert.ToString(filter(MOFCarbon)); // Массовая доля углерода
            textBoxMOF_H.Text = Convert.ToString(filter(MOFHydrogen)); // Массовая доля водорода
            textBoxMOF_O.Text = Convert.ToString(filter(MOFOxygen)); // Массовая доля кислорода
            //textBoxEps.Text = Convert.ToString(eps); // Точноcть

            textBoxSigmaInlet.Text = Convert.ToString(filter(eta[2])); // Коэффициент восстановления давления в ВУ

            textBoxPiFan.Text = Convert.ToString(filter(p3fp2d)); // Степень повышения давления в вентиляторе
            textBox_m.Text = Convert.ToString(filter(byprat)); // Степень двухконтурности
            textBoxEtaFan.Text = Convert.ToString(filter(eta[13])); // КПД вентилятора

            textBoxPiComp.Text = Convert.ToString(filter(p3p2d)); // Степень повышения давления в компрессоре
            textBoxEtaComp.Text = Convert.ToString(filter(eta[3])); // КПД компрессора

            textBoxT4.Text = Convert.ToString(filter(tt4d * R_K)); // Температура перед турбиной
            textBoxSigmaBurner.Text = Convert.ToString(filter(prat[4])); // Коэффициент восстановления давления в КС
            textBoxEtaBurner.Text = Convert.ToString(filter(eta[4])); // КПД сгорания

            textBoxEtaTurbine.Text = Convert.ToString(filter(eta[5])); // КПД турбины

            textBoxFiNozzle.Text = Convert.ToString(filter(eta[7])); // Коэффициент скорости сопла
            textBoxAfterBurner.Text = Convert.ToString(filter(tt7d * R_K)); // Температура ФК



            textBoxThrust.Text = Convert.ToString(filter(fnlb * lbs_N)); // Тяга
            textBoxGrossThrust.Text = Convert.ToString(filter(fglb * lbs_N)); // Полная тяга
            textBoxRamDrag.Text = Convert.ToString(filter(drlb * lbs_N)); // Лобовая сила
            textBox_q0.Text = Convert.ToString(filter(q0 * 47.880208)); // Скоростной напор
            textBoxThermalEff.Text = Convert.ToString(filter(eteng)); // Термический КПД

            textBoxThrustUdel.Text = Convert.ToString(filter(fnet * 9.806)); // Удельная тяга
            textBoxTSFC.Text = Convert.ToString(filter(sfc * forSFC)); // Удельный расход топлива
            textBoxEAir.Text = Convert.ToString(filter(eair * lbps_kgps)); // Расход воздуха
            textBoxGFuel.Text = Convert.ToString(filter(fuelrat * 0.453592)); // Расход топлива
            textBoxFuelAir.Text = Convert.ToString(filter(fa)); // Относительный расход топлива

            textBoxDiameterNozzle.Text = Convert.ToString(filter(Math.Sqrt(4.0 * a8 / Math.PI) * ft_m)); // Диаметр сопла
            textBoxVExit.Text = Convert.ToString(filter(uexit * ft_m)); // Скорость истечения из сопла
            textBoxTNozzle.Text = Convert.ToString(filter(t8 * R_K)); // Температура на срезе сопла
            textBoxPNozzle.Text = Convert.ToString(filter(pexit * lb_sq_inch__kPa)); // Давление на срезе сопла

            if (entype == 2)
                textBoxPFan.Text = Convert.ToString(filter(pfexit * lb_sq_inch__kPa)); // Давление на срезе вентилятора
            else
                textBoxPFan.Text = ""; // Давление на срезе вентилятора
        }
        #endregion

        #region Push Extra Data
        private void pushExtraData()
        {
            textBoxPt1.Text = Convert.ToString(filter(pt[1] * lb_sq_inch__kPa)); // P1
            textBoxPt2.Text = Convert.ToString(filter(pt[13] * lb_sq_inch__kPa)); // P2
            textBoxPt3.Text = Convert.ToString(filter(pt[3] * lb_sq_inch__kPa)); // P3
            textBoxPt4.Text = Convert.ToString(filter(pt[4] * lb_sq_inch__kPa)); // P4
            textBoxPt5.Text = Convert.ToString(filter(pt[5] * lb_sq_inch__kPa)); // P5
            textBoxPt6.Text = Convert.ToString(filter(pt[15] * lb_sq_inch__kPa)); // P6
            textBoxPt7.Text = Convert.ToString(filter(pt[7] * lb_sq_inch__kPa)); // P7
            textBoxPt8.Text = Convert.ToString(filter(pt[8] * lb_sq_inch__kPa)); // P8

            textBoxTt1.Text = Convert.ToString(filter(tt[1] * R_K)); // T1
            textBoxTt2.Text = Convert.ToString(filter(tt[13] * R_K)); // T2
            textBoxTt3.Text = Convert.ToString(filter(tt[3] * R_K)); // T3
            textBoxTt4.Text = Convert.ToString(filter(tt[4] * R_K)); // T4
            textBoxTt5.Text = Convert.ToString(filter(tt[5] * R_K)); // T5
            textBoxTt6.Text = Convert.ToString(filter(tt[15] * R_K)); // T6
            textBoxTt7.Text = Convert.ToString(filter(tt[7] * R_K)); // T7
            textBoxTt8.Text = Convert.ToString(filter(tt[8] * R_K)); // T8
        }
        #endregion



        #region Get Gamma, Cp, Mach, RayleighLoss, Air, Fuel, Free Stream, Thermo, Geometry, Engine Perform

        #region Get Gamma
        private double getGama(double temp, int opt)
        {
            // Utility to get gamma as a function of temp 
            double number, a, b, c, d;
            a = -7.6942651e-13;
            b = 1.3764661e-08;
            c = -7.8185709e-05;
            d = 1.436914;
            if (opt == 0)
            {
                number = 1.4;
            }
            else
            {
                number = a * temp * temp * temp + b * temp * temp + c * temp + d;
            }
            return number;
        }
        #endregion

        #region Get Cp
        private double getCp(double temp, int opt)
        {
            // Utility to get cp as a function of temp 
            double number, a, b, c, d;
            /* BTU/R */
            a = -4.4702130e-13;
            b = -5.1286514e-10;
            c = 2.8323331e-05;
            d = 0.2245283;
            if (opt == 0)
            {
                number = 0.2399;
            }
            else
            {
                number = a * temp * temp * temp + b * temp * temp + c * temp + d;
            }
            return number;
        }
        #endregion

        #region Get Mach
        private double getMach(int sub, double corair, double gamma)
        {
            /* Utility to get the Mach number given the corrected airflow per area */
            double number, chokair;              /* iterate for mach number */
            double deriv, machn, macho, airo, airn;
            int iter;

            chokair = getAir(1.0, gamma);
            if (corair > chokair)
            {
                number = 1.0;
                return number;
            }
            else
            {
                airo = 0.25618;                 /* initial guess */
                if (sub == 1) macho = 1.0;   /* sonic */
                else
                {
                    if (sub == 2) macho = 1.703; /* supersonic */
                    else macho = .5;                /* subsonic */
                    iter = 1;
                    machn = macho - .2;
                    while (Math.Abs(corair - airo) > .0001 && iter < 20)
                    {
                        airn = getAir(machn, gamma);
                        deriv = (airn - airo) / (machn - macho);
                        airo = airn;
                        macho = machn;
                        machn = macho + (corair - airo) / deriv;
                        ++iter;
                    }
                }
                number = macho;
            }
            return number;
        }
        #endregion

        #region Get RayleighLoss
        private double getRayleighLoss(double mach1, double ttrat, double tlow)
        {
            /* analysis for rayleigh flow */
            double number;
            double wc1, wc2, mgueso, mach2, g1, gm1, g2, gm2;
            double fac1, fac2, fac3, fac4;

            g1 = getGama(tlow, gamopt);
            gm1 = g1 - 1.0;
            wc1 = getAir(mach1, g1);
            g2 = getGama(tlow * ttrat, gamopt);
            gm2 = g2 - 1.0;
            number = 0.95;
            /* iterate for mach downstream */
            mgueso = 0.4;                 /* initial guess */
            mach2 = 0.5;
            while (Math.Abs(mach2 - mgueso) > .0001)
            {
                mgueso = mach2;
                fac1 = 1.0 + g1 * mach1 * mach1;
                fac2 = 1.0 + g2 * mach2 * mach2;
                fac3 = Math.Pow((1.0 + .5 * gm1 * mach1 * mach1), (g1 / gm1));
                fac4 = Math.Pow((1.0 + .5 * gm2 * mach2 * mach2), (g2 / gm2));
                number = fac1 * fac4 / fac2 / fac3;
                wc2 = wc1 * Math.Sqrt(ttrat) / number;
                mach2 = getMach(0, wc2, g2);
            }
            return number;
        }
        #endregion

        #region Get Air
        private double getAir(double mach, double gamma)
        {
            /* Utility to get the corrected airflow per area given the Mach number */
            double number, fac1, fac2;
            fac2 = (gamma + 1.0) / (2.0 * (gamma - 1.0));
            fac1 = Math.Pow((1.0 + .5 * (gamma - 1.0) * mach * mach), fac2);
            number = .50161 * Math.Sqrt(gamma) * mach / fac1;

            return number;
        }
        #endregion

        #region Get Fuel
        private void getFuel()
        {
            if (fueltype == 0)
                fhv = 18600.0;
            if (fueltype == 1)
                fhv = 49900.0;
            if (fueltype == 2)
                fhv = Convert.ToDouble(textBoxHu.Text) / forHu;
            if (fueltype == 3)
            {
                MOFCarbon = Convert.ToDouble(textBoxMOF_C.Text);
                MOFHydrogen = Convert.ToDouble(textBoxMOF_H.Text);
                MOFOxygen = Convert.ToDouble(textBoxMOF_O.Text);
                fhv = 33900.0 * MOFCarbon + 103100.0 * MOFHydrogen - 10900.0 * MOFOxygen;
            }
            textBoxHu.Text = Convert.ToString(filter(fhv * forHu)); // Низшая теплота сгорания
        }
        #endregion

        #region Get Free Stream
        private void getFreeStream()
        {
            rgas = 1718.0;                /* ft2/sec2 R */
            if (inptype >= 2)
            {
                ps0 = ps0 * 144.0;
            }
            if (inptype <= 1)
            {            /* input altitude */
                alt = altd / lconv1;
                if (alt < 36152.0)
                {
                    ts0 = 518.6 - 3.56 * alt / 1000.0;
                    ps0 = 2116.0 * Math.Pow(ts0 / 518.6, 5.256);
                }
                if (alt >= 36152.0 && alt <= 82345.0)
                {   // Stratosphere
                    ts0 = 389.98;
                    ps0 = 2116.0 * 0.2236 * Math.Exp((36000.0 - alt) / (53.35 * 389.98));
                }
                if (alt >= 82345.0)
                {
                    ts0 = 389.98 + 1.645 * (alt - 82345) / 1000.0;
                    ps0 = 2116.0 * 0.02456 * Math.Pow(ts0 / 389.98, -11.388);
                }
            }
            a0 = Math.Sqrt(gama * rgas * ts0);             /* speed of sound ft/sec */
            if (inptype == 0 || inptype == 2)
            {           /* input speed  */
                u0 = u0d / lconv2 * 5280.0 / 3600.0;           /* airspeed ft/sec */
                fsmach = u0 / a0;
                q0 = gama / 2.0 * fsmach * fsmach * ps0;
            }
            if (inptype == 1 || inptype == 3)
            {            /* input mach */
                u0 = fsmach * a0;
                u0d = u0 * lconv2 / 5280.0 * 3600.0;      /* airspeed ft/sec */
                q0 = gama / 2.0 * fsmach * fsmach * ps0;
            }
            if (u0 > 0.0001) rho0 = q0 / (u0 * u0);
            else rho0 = 1.0;

            tt[0] = ts0 * (1.0 + 0.5 * (gama - 1.0) * fsmach * fsmach);
            pt[0] = ps0 * Math.Pow(tt[0] / ts0, gama / (gama - 1.0));
            ps0 = ps0 / 144.0;
            pt[0] = pt[0] / 144.0;
            cpair = getCp(tt[0], gamopt);              /*BTU/lbm R */
            tsout = ts0;
            psout = ps0;
        }
        #endregion

        #region Get Thermo
        public void getThermo()
        {
            double dela, t5t4n, deriv, delan, m5;
            double delhc, delhht, delhf, delhlt;
            double deltc, deltht, deltf, deltlt;
            int itcount, index;
            float fl1;
            int i1;
            /*   inlet recovery  */
            if (pt2flag == 0)
            {                    /*     mil spec      */
                if (fsmach > 1.0)
                {          /* supersonic */
                    prat[2] = 1.0 - .075 * Math.Pow(fsmach - 1.0, 1.35);
                }
                else
                {
                    prat[2] = 1.0;
                }
                eta[2] = prat[2];
            }
            else
            {                       /* enter value */
                prat[2] = eta[2];
            }
            /* protection for overwriting input */
            if (eta[3] < .5) eta[3] = .5;
            if (eta[5] < .5) eta[5] = .5;
            trat[7] = 1.0;
            prat[7] = 1.0;
            tt[2] = tt[1] = tt[0];
            pt[1] = pt[0];
            gam[2] = getGama(tt[2], gamopt);
            cp[2] = getCp(tt[2], gamopt);
            pt[2] = pt[1] * prat[2];
            /* design - p3p2 specified - tt4 specified */
            if (inflag == 0)
            {
                if (entype <= 1)
                {              /* turbojet */
                    prat[3] = p3p2d;                      /* core compressor */
                    if (prat[3] < .5)
                        prat[3] = .5;

                    delhc = (cp[2] * tt[2] / eta[3]) *
                          (Math.Pow(prat[3], (gam[2] - 1.0) / gam[2]) - 1.0);

                    deltc = delhc / cp[2];
                    pt[3] = pt[2] * prat[3];
                    tt[3] = tt[2] + deltc;
                    trat[3] = tt[3] / tt[2];
                    gam[3] = getGama(tt[3], gamopt);
                    cp[3] = getCp(tt[3], gamopt);
                    tt[4] = tt4 * throtl / 100.0;
                    gam[4] = getGama(tt[4], gamopt);
                    cp[4] = getCp(tt[4], gamopt);
                    trat[4] = tt[4] / tt[3];
                    pt[4] = pt[3] * prat[4];
                    delhht = delhc;
                    deltht = delhht / cp[4];
                    tt[5] = tt[4] - deltht;
                    gam[5] = getGama(tt[5], gamopt);
                    cp[5] = getCp(tt[5], gamopt);
                    trat[5] = tt[5] / tt[4];
                    prat[5] = Math.Pow((1.0 - delhht / cp[4] / tt[4] / eta[5]),
                             (gam[4] / (gam[4] - 1.0)));
                    pt[5] = pt[4] * prat[5];
                    /* fan conditions */
                    prat[13] = 1.0;
                    trat[13] = 1.0;
                    tt[13] = tt[2];
                    pt[13] = pt[2];
                    gam[13] = gam[2];
                    cp[13] = cp[2];
                    prat[15] = 1.0;
                    pt[15] = pt[5];
                    trat[15] = 1.0;
                    tt[15] = tt[5];
                    gam[15] = gam[5];
                    cp[15] = cp[5];
                }

                if (entype == 2)
                {                         /* turbofan */
                    prat[13] = p3fp2d;
                    if (prat[13] < .5) prat[13] = .5;
                    delhf = (cp[2] * tt[2] / eta[13]) *
                          (Math.Pow(prat[13], (gam[2] - 1.0) / gam[2]) - 1.0);
                    deltf = delhf / cp[2];
                    tt[13] = tt[2] + deltf;
                    pt[13] = pt[2] * prat[13];
                    trat[13] = tt[13] / tt[2];
                    gam[13] = getGama(tt[13], gamopt);
                    cp[13] = getCp(tt[13], gamopt);
                    prat[3] = p3p2d;                      /* core compressor */
                    if (prat[3] < .5) prat[3] = .5;
                    delhc = (cp[13] * tt[13] / eta[3]) *
                          (Math.Pow(prat[3], (gam[13] - 1.0) / gam[13]) - 1.0);
                    deltc = delhc / cp[13];
                    tt[3] = tt[13] + deltc;
                    pt[3] = pt[13] * prat[3];
                    trat[3] = tt[3] / tt[13];
                    gam[3] = getGama(tt[3], gamopt);
                    cp[3] = getCp(tt[3], gamopt);
                    tt[4] = tt4 * throtl / 100.0;
                    pt[4] = pt[3] * prat[4];
                    gam[4] = getGama(tt[4], gamopt);
                    cp[4] = getCp(tt[4], gamopt);
                    trat[4] = tt[4] / tt[3];
                    delhht = delhc;
                    deltht = delhht / cp[4];
                    tt[5] = tt[4] - deltht;
                    gam[5] = getGama(tt[5], gamopt);
                    cp[5] = getCp(tt[5], gamopt);
                    trat[5] = tt[5] / tt[4];
                    prat[5] = Math.Pow((1.0 - delhht / cp[4] / tt[4] / eta[5]),
                                (gam[4] / (gam[4] - 1.0)));
                    pt[5] = pt[4] * prat[5];
                    delhlt = (1.0 + byprat) * delhf;
                    deltlt = delhlt / cp[5];
                    tt[15] = tt[5] - deltlt;
                    gam[15] = getGama(tt[15], gamopt);
                    cp[15] = getCp(tt[15], gamopt);
                    trat[15] = tt[15] / tt[5];
                    prat[15] = Math.Pow((1.0 - delhlt / cp[5] / tt[5] / eta[5]),
                                (gam[5] / (gam[5] - 1.0)));
                    pt[15] = pt[5] * prat[15];
                }

                if (entype == 3)
                {              /* ramjet */
                    prat[3] = 1.0;
                    pt[3] = pt[2] * prat[3];
                    tt[3] = tt[2];
                    trat[3] = 1.0;
                    gam[3] = getGama(tt[3], gamopt);
                    cp[3] = getCp(tt[3], gamopt);
                    tt[4] = tt4 * throtl / 100.0;
                    gam[4] = getGama(tt[4], gamopt);
                    cp[4] = getCp(tt[4], gamopt);
                    trat[4] = tt[4] / tt[3];
                    pt[4] = pt[3] * prat[4];
                    tt[5] = tt[4];
                    gam[5] = getGama(tt[5], gamopt);
                    cp[5] = getCp(tt[5], gamopt);
                    trat[5] = 1.0;
                    prat[5] = 1.0;
                    pt[5] = pt[4];
                    /* fan conditions */
                    prat[13] = 1.0;
                    trat[13] = 1.0;
                    tt[13] = tt[2];
                    pt[13] = pt[2];
                    gam[13] = gam[2];
                    cp[13] = cp[2];
                    prat[15] = 1.0;
                    pt[15] = pt[5];
                    trat[15] = 1.0;
                    tt[15] = tt[5];
                    gam[15] = gam[5];
                    cp[15] = cp[5];
                }

                tt[7] = tt7;
            }
            /* analysis -assume flow choked at both turbine entrances */
            /* and nozzle throat ... then*/
            else
            {
                tt[4] = tt4 * throtl / 100.0;
                gam[4] = getGama(tt[4], gamopt);
                cp[4] = getCp(tt[4], gamopt);
                if (a4 < .02) a4 = .02;

                if (entype <= 1)
                {              /* turbojet */
                    dela = .2;                           /* iterate to get t5t4 */
                    trat[5] = 1.0;
                    t5t4n = .5;
                    itcount = 0;
                    while (Math.Abs(dela) > .001 && itcount < 20)
                    {
                        ++itcount;
                        delan = a8d / a4 - Math.Sqrt(t5t4n) *
                             Math.Pow((1.0 - (1.0 / eta[5]) * (1.0 - t5t4n)),
                                    -gam[4] / (gam[4] - 1.0));
                        deriv = (delan - dela) / (t5t4n - trat[5]);
                        dela = delan;
                        trat[5] = t5t4n;
                        t5t4n = trat[5] - dela / deriv;
                    }
                    tt[5] = tt[4] * trat[5];
                    gam[5] = getGama(tt[5], gamopt);
                    cp[5] = getCp(tt[5], gamopt);
                    deltht = tt[5] - tt[4];
                    delhht = cp[4] * deltht;
                    prat[5] = Math.Pow((1.0 - (1.0 / eta[5]) * (1.0 - trat[5])),
                                 gam[4] / (gam[4] - 1.0));
                    delhc = delhht;           /* compressor work */
                    deltc = -delhc / cp[2];
                    tt[3] = tt[2] + deltc;
                    gam[3] = getGama(tt[3], gamopt);
                    cp[3] = getCp(tt[3], gamopt);
                    trat[3] = tt[3] / tt[2];
                    prat[3] = Math.Pow((1.0 + eta[3] * (trat[3] - 1.0)),
                                 gam[2] / (gam[2] - 1.0));
                    trat[4] = tt[4] / tt[3];
                    pt[3] = pt[2] * prat[3];
                    pt[4] = pt[3] * prat[4];
                    pt[5] = pt[4] * prat[5];
                    /* fan conditions */
                    prat[13] = 1.0;
                    trat[13] = 1.0;
                    tt[13] = tt[2];
                    pt[13] = pt[2];
                    gam[13] = gam[2];
                    cp[13] = cp[2];
                    prat[15] = 1.0;
                    pt[15] = pt[5];
                    trat[15] = 1.0;
                    tt[15] = tt[5];
                    gam[15] = gam[5];
                    cp[15] = cp[5];
                }

                if (entype == 2)
                {                        /*  turbofan */
                    dela = .2;                           /* iterate to get t5t4 */
                    trat[5] = 1.0;
                    t5t4n = .5;
                    itcount = 0;
                    while (Math.Abs(dela) > .001 && itcount < 20)
                    {
                        ++itcount;
                        delan = a4p / a4 - Math.Sqrt(t5t4n) *
                                Math.Pow((1.0 - (1.0 / eta[5]) * (1.0 - t5t4n)),
                                    -gam[4] / (gam[4] - 1.0));
                        deriv = (delan - dela) / (t5t4n - trat[5]);
                        dela = delan;
                        trat[5] = t5t4n;
                        t5t4n = trat[5] - dela / deriv;
                    }
                    tt[5] = tt[4] * trat[5];
                    gam[5] = getGama(tt[5], gamopt);
                    cp[5] = getCp(tt[5], gamopt);
                    deltht = tt[5] - tt[4];
                    delhht = cp[4] * deltht;
                    prat[5] = Math.Pow((1.0 - (1.0 / eta[5]) * (1.0 - trat[5])),
                              gam[4] / (gam[4] - 1.0));
                    /* iterate to get t15t14 */
                    dela = .2;
                    trat[15] = 1.0;
                    t5t4n = .5;
                    itcount = 0;
                    while (Math.Abs(dela) > .001 && itcount < 20)
                    {
                        ++itcount;
                        delan = a8d / a4p - Math.Sqrt(t5t4n) *
                                 Math.Pow((1.0 - (1.0 / eta[5]) * (1.0 - t5t4n)),
                                   -gam[5] / (gam[5] - 1.0));
                        deriv = (delan - dela) / (t5t4n - trat[15]);
                        dela = delan;
                        trat[15] = t5t4n;
                        t5t4n = trat[15] - dela / deriv;
                    }
                    tt[15] = tt[5] * trat[15];
                    gam[15] = getGama(tt[15], gamopt);
                    cp[15] = getCp(tt[15], gamopt);
                    deltlt = tt[15] - tt[5];
                    delhlt = cp[5] * deltlt;
                    prat[15] = Math.Pow((1.0 - (1.0 / eta[5]) * (1.0 - trat[15])),
                                gam[5] / (gam[5] - 1.0));
                    byprat = afan / acore - 1.0;
                    delhf = delhlt / (1.0 + byprat);              /* fan work */
                    deltf = -delhf / cp[2];
                    tt[13] = tt[2] + deltf;
                    gam[13] = getGama(tt[13], gamopt);
                    cp[13] = getCp(tt[13], gamopt);
                    trat[13] = tt[13] / tt[2];
                    prat[13] = Math.Pow((1.0 + eta[13] * (trat[13] - 1.0)),
                              gam[2] / (gam[2] - 1.0));
                    delhc = delhht;                         /* compressor work */
                    deltc = -delhc / cp[13];
                    tt[3] = tt[13] + deltc;
                    gam[3] = getGama(tt[3], gamopt);
                    cp[3] = getCp(tt[3], gamopt);
                    trat[3] = tt[3] / tt[13];
                    prat[3] = Math.Pow((1.0 + eta[3] * (trat[3] - 1.0)),
                                 gam[13] / (gam[13] - 1.0));
                    trat[4] = tt[4] / tt[3];
                    pt[13] = pt[2] * prat[13];
                    pt[3] = pt[13] * prat[3];
                    pt[4] = pt[3] * prat[4];
                    pt[5] = pt[4] * prat[5];
                    pt[15] = pt[5] * prat[15];
                }

                if (entype == 3)
                {              /* ramjet */
                    prat[3] = 1.0;
                    pt[3] = pt[2] * prat[3];
                    tt[3] = tt[2];
                    trat[3] = 1.0;
                    gam[3] = getGama(tt[3], gamopt);
                    cp[3] = getCp(tt[3], gamopt);
                    tt[4] = tt4 * throtl / 100.0;
                    trat[4] = tt[4] / tt[3];
                    pt[4] = pt[3] * prat[4];
                    tt[5] = tt[4];
                    gam[5] = getGama(tt[5], gamopt);
                    cp[5] = getCp(tt[5], gamopt);
                    trat[5] = 1.0;
                    prat[5] = 1.0;
                    pt[5] = pt[4];
                    /* fan conditions */
                    prat[13] = 1.0;
                    trat[13] = 1.0;
                    tt[13] = tt[2];
                    pt[13] = pt[2];
                    gam[13] = gam[2];
                    cp[13] = cp[2];
                    prat[15] = 1.0;
                    pt[15] = pt[5];
                    trat[15] = 1.0;
                    tt[15] = tt[5];
                    gam[15] = gam[5];
                    cp[15] = cp[5];
                }

                if (abflag == 1) tt[7] = tt7;
            }

            prat[6] = 1.0;
            pt[6] = pt[15];
            trat[6] = 1.0;
            tt[6] = tt[15];
            gam[6] = getGama(tt[6], gamopt);
            cp[6] = getCp(tt[6], gamopt);
            if (abflag > 0)
            {                   /* afterburner */
                trat[7] = tt[7] / tt[6];
                m5 = getMach(0, getAir(1.0, gam[5]) * a4 / acore, gam[5]);
                prat[7] = getRayleighLoss(m5, trat[7], tt[6]);
            }
            tt[7] = tt[6] * trat[7];
            pt[7] = pt[6] * prat[7];
            gam[7] = getGama(tt[7], gamopt);
            cp[7] = getCp(tt[7], gamopt);
            /* engine press ratio EPR*/
            epr = prat[7] * prat[15] * prat[5] * prat[4] * prat[3] * prat[13];
            /* engine temp ratio ETR */
            etr = trat[7] * trat[15] * trat[5] * trat[4] * trat[3] * trat[13];
        }
        #endregion

        #region Get Geometry
        public void getGeo()
        {
            // Сколько ступеней в компрессоре
            ncomp = (int)(1.0 + p3p2d / 1.5);
            if (ncomp > 15)
                ncomp = 15;

            // Сколько ступеней в турбине
            nturb = 1 + ncomp / 4;
            if (entype == 2)
                nturb = nturb + 1;

            a2d = 3.14159 * diameng * diameng / 4.0;
            a2 = a2d;

            if (entype == 2)
            {
                afan = a2;
                acore = afan / (1.0 + byprat);
            }
            else acore = a2;


            /* determine geometric variables */
            double game;
            float fl1;
            int i1;

            if (entype <= 2)
            {          // turbine engines
                if (afan < acore) afan = acore;
                a8max = .75 * Math.Sqrt(etr) / epr; /* limits compressor face  */
                /*  mach number  to < .5   */
                if (a8max > 1.0) a8max = 1.0;
                if (a8rat > a8max)
                {
                    a8rat = a8max;
                }
                /*    dumb down limit - a8 schedule */
                if (arsched == 0)
                {
                    a8rat = a8max;
                }
                a8 = a8rat * acore;
                a8d = a8 * prat[7] / Math.Sqrt(trat[7]);
                /* assumes choked a8 and a4 */
                a4 = a8 * prat[5] * prat[15] * prat[7] /
                 Math.Sqrt(trat[7] * trat[5] * trat[15]);
                a4p = a8 * prat[15] * prat[7] / Math.Sqrt(trat[7] * trat[15]);
                ac = 0.9 * a2;
            }

            if (entype == 3)
            {      // ramjets
                game = getGama(tt[4], gamopt);
                if (athsched == 0)
                {   // scheduled throat area
                    arthd = getAir(fsmach, gama) * Math.Sqrt(etr) /
                            (getAir(1.0, game) * epr * prat[2]);
                    if (arthd < arthmn) arthd = arthmn;
                    if (arthd > arthmx) arthd = arthmx;
                }
                if (aexsched == 0)
                {   // scheduled exit area
                    mexit = Math.Sqrt((2.0 / (game - 1.0)) * ((1.0 + .5 * (gama - 1.0) * fsmach * fsmach)
                         * Math.Pow((epr * prat[2]), (game - 1.0) / game) - 1.0));
                    arexitd = getAir(1.0, game) / getAir(mexit, game);
                    if (arexitd < arexmn) arexitd = arexmn;
                    if (arexitd > arexmx) arexitd = arexmx;
                }
            }
        }
        #endregion

        #region Get Engine Perform
        private void getPerform()
        {       /* determine engine performance */
            double fac1, game, cpe, cp3, rg, p8p5, rg1;
            int index;

            rg1 = 53.3;
            rg = cpair * (gama - 1.0) / gama;
            cp3 = getCp(tt[3], gamopt);                  /*BTU/lbm R */
            g0 = 32.2;
            ues = 0.0;
            game = getGama(tt[5], gamopt);
            fac1 = (game - 1.0) / game;
            cpe = getCp(tt[5], gamopt);
            if (eta[7] < 0.8) eta[7] = 0.8;    /* protection during overwriting */
            if (eta[4] < 0.8) eta[4] = 0.8;

            /*  specific net thrust  - thrust / (g0*airflow) -   lbf/lbm/sec  */
            // turbine engine core
            if (entype <= 2)
            {
                /* airflow determined at choked nozzle exit */
                pt[8] = epr * prat[2] * pt[0];
                eair = getAir(1.0, game) * 144.0 * a8 * pt[8] / 14.7 /
                        Math.Sqrt(etr * tt[0] / 518.0);
                m2 = getMach(0, eair * Math.Sqrt(tt[0] / 518.0) /
                        (prat[2] * pt[0] / 14.7 * acore * 144.0), gama);
                npr = pt[8] / ps0;
                uexit = Math.Sqrt(2.0 * rgas / fac1 * etr * tt[0] * eta[7] *
                        (1.0 - Math.Pow(1.0 / npr, fac1)));
                if (npr <= 1.893) pexit = ps0;
                else pexit = .52828 * pt[8];
                fgros = (uexit + (pexit - ps0) * 144.0 * a8 / eair) / g0;
            }

            // turbo fan -- added terms for fan flow
            if (entype == 2)
            {
                fac1 = (gama - 1.0) / gama;
                snpr = pt[13] / ps0;
                ues = Math.Sqrt(2.0 * rgas / fac1 * tt[13] * eta[7] *
                         (1.0 - Math.Pow(1.0 / snpr, fac1)));
                m2 = getMach(0, eair * (1.0 + byprat) * Math.Sqrt(tt[0] / 518.0) /
                         (prat[2] * pt[0] / 14.7 * afan * 144.0), gama); //
                if (snpr <= 1.893)
                    pfexit = ps0;
                else
                    pfexit = 0.52828 * pt[13];
                fgros += (byprat * ues + (pfexit - ps0) * 144.0 * byprat * acore / eair) / g0; //
            }

            // ramjets
            if (entype == 3)
            {
                /* airflow determined at nozzle throat */
                eair = getAir(1.0, game) * 144.0 * a2 * arthd * epr * prat[2] * pt[0] / 14.7 /
                        Math.Sqrt(etr * tt[0] / 518.0);
                m2 = getMach(0, eair * Math.Sqrt(tt[0] / 518.0) /
                        (prat[2] * pt[0] / 14.7 * afan * 144.0), gama);
                mexit = getMach(2, (getAir(1.0, game) / arexitd), game);
                uexit = mexit * Math.Sqrt(game * rgas * etr * tt[0] * eta[7] /
                         (1.0 + .5 * (game - 1.0) * mexit * mexit));
                pexit = Math.Pow((1.0 + .5 * (game - 1.0) * mexit * mexit), (-game / (game - 1.0)))
                         * epr * prat[2] * pt[0];
                fgros = (uexit + (pexit - ps0) * arexitd * arthd * a2 / eair / 144.0) / g0;
            }

            // ram drag
            dram = u0 / g0;
            if (entype == 2)
                dram += u0 * byprat / g0;
            // mass flow ratio
            if (fsmach > 0.01)
                mfr = getAir(m2, gama) * prat[2] / getAir(fsmach, gama);
            else
                mfr = 5.0;

            // net thrust
            fnet = fgros - dram;
            if (entype == 3 && fsmach < .3)
            {
                fnet = 0.0;
                fgros = 0.0;
            }

            // thrust in pounds
            fnlb = fnet * eair; // чистая тяга
            fglb = fgros * eair; // полная тяга
            drlb = dram * eair; // лобовое сопротивление

            //fuel-air ratio and sfc
            fa = (trat[4] - 1.0) / (eta[4] * fhv / (cp3 * tt[3]) - trat[4]) +
              (trat[7] - 1.0) / (fhv / (cpe * tt[15]) - trat[7]);
            if (fnet > 0.0)
            {
                sfc = 3600.0 * fa / fnet;
                flflo = sfc * fnlb;
                isp = (fnlb / flflo) * 3600.0;
            }
            else
            {
                fnlb = 0.0;
                flflo = 0.0;
                sfc = 0.0;
                isp = 0.0;
            }
            tt[8] = tt[7];
            t8 = etr * tt[0] - uexit * uexit / (2.0 * rgas * game / (game - 1.0));
            trat[8] = tt[8] / tt[7];
            p8p5 = ps0 / (epr * prat[2] * pt[0]);
            cp[8] = getCp(tt[8], gamopt);
            pt[8] = pt[7];
            prat[8] = pt[8] / pt[7];
            /* thermal effeciency */
            if (entype == 2)
            {
                eteng = (a0 * a0 * ((1.0 + fa) * (uexit * uexit / (a0 * a0))
                + byprat * (ues * ues / (a0 * a0)) - (1.0 + byprat) * fsmach * fsmach)) / (2.0 * g0 * fa * fhv * 778.16);
            }
            else
            {
                eteng = (a0 * a0 * ((1.0 + fa) * (uexit * uexit / (a0 * a0))
                - fsmach * fsmach)) / (2.0 * g0 * fa * fhv * 778.16);
            }

            s[0] = s[1] = .2;
            v[0] = v[1] = rg1 * ts0 / (ps0 * 144.0);
            for (index = 2; index <= 7; ++index)
            {     /* compute entropy */
                s[index] = s[index - 1] + cpair * Math.Log(trat[index])
                                      - rg * Math.Log(prat[index]);
                v[index] = rg1 * tt[index] / (pt[index] * 144.0);
            }
            s[13] = s[2] + cpair * Math.Log(trat[13]) - rg * Math.Log(prat[13]);
            v[13] = rg1 * tt[13] / (pt[13] * 144.0);
            s[15] = s[5] + cpair * Math.Log(trat[15]) - rg * Math.Log(prat[15]);
            v[15] = rg1 * tt[15] / (pt[15] * 144.0);
            s[8] = s[7] + cpair * Math.Log(t8 / (etr * tt[0])) - rg * Math.Log(p8p5);
            v[8] = rg1 * t8 / (ps0 * 144.0);
            cp[0] = getCp(tt[0], gamopt);

            fntot = numeng * fnlb;
            fuelrat = numeng * flflo;
            // weight  calculation
            if (wtflag == 0)
            {
                if (entype == 0)
                {
                    //            weight = .0838 * acore * (dcomp * ncomp + dburner +
                    //                     dturbin * nturb + dburner) * Math.sqrt(acore / 1.753) ;
                    weight = 0.132 * Math.Sqrt(acore * acore * acore) *
                     (dcomp * lcomp + dburner * lburn + dturbin * lturb + dnozl * lnoz);
                }
                if (entype == 1)
                {
                    //            weight = .0838 * acore * (dcomp * ncomp + dburner +
                    //                 dturbin * nturb + dburner * 2.0) * Math.sqrt(acore / 6.00) ;
                    weight = 0.100 * Math.Sqrt(acore * acore * acore) *
                     (dcomp * lcomp + dburner * lburn + dturbin * lturb + dnozl * lnoz);
                }
                if (entype == 2)
                {
                    weight = 0.0932 * acore * ((1.0 + byprat) * dfan * 4.0 + dcomp * (ncomp - 3) +
                              dburner + dturbin * nturb + dnozl * 2.0) * Math.Sqrt(acore / 6.965);
                }
                if (entype == 3)
                {
                    weight = 0.1242 * acore * (dburner + dnozr * 6.0 + dinlt * 3.0) * Math.Sqrt(acore / 1.753);
                }
            }
        }
        #endregion

        #endregion

        #region Input Type, Engine Type, Fuel Type

        #region Input Type
        private void comboBoxInputType_TextChanged(object sender, EventArgs e)
        {
            switch (comboBoxInputType.Text)
            {
                case "Расчет по Скорости(V) и Высоте(H)":
                    {
                        inptype = 0;
                        textBoxVFlight.Enabled = true;
                        textBoxAltitude.Enabled = true;
                        textBoxMachNumber.Enabled = false;
                        textBoxT0.Enabled = false;
                        textBoxP0.Enabled = false;
                    }
                    break;
                case "Расчет по числу Маха(M) и Высоте(H)":
                    {
                        inptype = 1;
                        textBoxVFlight.Enabled = false;
                        textBoxAltitude.Enabled = true;
                        textBoxMachNumber.Enabled = true;
                        textBoxT0.Enabled = false;
                        textBoxP0.Enabled = false;
                    }
                    break;
                case "Расчет по Скорости(V), Давлению(P0) и Температуре(T0)":
                    {
                        inptype = 2;
                        textBoxVFlight.Enabled = true;
                        textBoxAltitude.Enabled = false;
                        textBoxMachNumber.Enabled = false;
                        textBoxT0.Enabled = true;
                        textBoxP0.Enabled = true;
                    }
                    break;
                case "Расчет по числу Маха(M), Давлению(P0) и Температуре(T0)":
                    {
                        inptype = 3;
                        textBoxVFlight.Enabled = false;
                        textBoxAltitude.Enabled = false;
                        textBoxMachNumber.Enabled = true;
                        textBoxT0.Enabled = true;
                        textBoxP0.Enabled = true;
                    }
                    break;
                default:
                    break;
            }
            firstPageDesign();
        }
        #endregion

        #region Engine Type
        private void comboBoxEngineType_TextChanged(object sender, EventArgs e)
        {
            switch (comboBoxEngineType.Text)
            {
                case "ТРД":
                    {
                        entype = 0;
                        abflag = 0;
                        textBoxPiFan.Enabled = false;
                        textBox_m.Enabled = false;
                        textBoxEtaFan.Enabled = false;
                        textBoxPFan.Enabled = false;
                        textBoxAfterBurner.Enabled = false;
                    }
                    break;
                case "ТРДФ":
                    {
                        entype = 1;
                        abflag = 1;
                        textBoxPiFan.Enabled = false;
                        textBox_m.Enabled = false;
                        textBoxEtaFan.Enabled = false;
                        textBoxPFan.Enabled = false;
                        textBoxAfterBurner.Enabled = true;
                    }
                    break;
                case "ТРДД":
                    {
                        entype = 2;
                        abflag = 0;
                        textBoxPiFan.Enabled = true;
                        textBox_m.Enabled = true;
                        textBoxEtaFan.Enabled = true;
                        textBoxPFan.Enabled = true;
                        textBoxAfterBurner.Enabled = false;
                    }
                    break;
                default:
                    break;
            }
            firstPageDesign();
        }
        #endregion

        #region Fuel Type
        private void comboBoxFuelType_TextChanged(object sender, EventArgs e)
        {
            switch (comboBoxFuelType.Text)
            {
                case "Jet - A":
                    {
                        fueltype = 0;
                        textBoxMOF_C.Enabled = false;
                        textBoxMOF_H.Enabled = false;
                        textBoxMOF_O.Enabled = false;
                        textBoxHu.Enabled = false;
                        textBoxHu.Text = Convert.ToString(filter(18600.0 * forHu));
                    }
                    break;
                case "Hydrogen":
                    {
                        fueltype = 1;
                        textBoxMOF_C.Enabled = false;
                        textBoxMOF_H.Enabled = false;
                        textBoxMOF_O.Enabled = false;
                        textBoxHu.Enabled = false;
                        textBoxHu.Text = Convert.ToString(filter(49900.0 * forHu));
                    }
                    break;
                case "Задать Hu":
                    {
                        fueltype = 2;
                        textBoxMOF_C.Enabled = false;
                        textBoxMOF_H.Enabled = false;
                        textBoxMOF_O.Enabled = false;
                        textBoxHu.Enabled = true;
                    }
                    break;
                case "Рассчитать Hu":
                    {
                        fueltype = 3;
                        textBoxMOF_C.Enabled = true;
                        textBoxMOF_H.Enabled = true;
                        textBoxMOF_O.Enabled = true;
                        textBoxHu.Enabled = false;
                    }
                    break;
                default:
                    break;
            }
            firstPageDesign();
        }
        #endregion

        #endregion



        #region Set Defaults NASA Design
        private void setDefaultsNASA()
        {
            int i;

            move = 0;
            inptype = 0;
            lconv1 = 1.0; lconv2 = 1.0; fconv = 1.0; mconv1 = 1.0;
            pconv = 1.0; econv = 1.0; aconv = 1.0; bconv = 1.0;
            mconv2 = 1.0; dconv = 1.0; flconv = 1.0; econv2 = 1.0;
            tconv = 1.0; tref = 459.6;
            g0 = g0d = 32.2;

            inflag = 0;
            counter = 0;
            showcom = 0;
            plttyp = 3;
            pltkeep = 0;
            entype = 0;
            varflag = 0;
            pt2flag = 0;
            wtflag = 0;
            fireflag = 0;
            gama = 1.4;
            gamopt = 1;
            u0d = 0.0;
            altd = 0.0;
            throtl = 100.0;

            for (i = 0; i <= 19; ++i)
            {
                trat[i] = 1.0;
                tt[i] = 518.6;
                prat[i] = 1.0;
                pt[i] = 14.7;
                eta[i] = 1.0;
            }

            tt[4] = tt4 = tt4d = 2500.0;
            tt[7] = tt7 = tt7d = 2500.0;
            prat[3] = p3p2d = 8.0;
            prat[13] = p3fp2d = 2.0;
            byprat = 1.0;
            abflag = 0;

            fueltype = 0;
            fhvd = fhv = 18600.0;
            a2d = a2 = acore = 2.0;
            diameng = Math.Sqrt(4.0 * a2d / 3.14159);
            ac = 0.9 * a2;
            a8rat = .35;
            a8 = 0.7;
            a8d = 0.4;
            arsched = 0;
            afan = 2.0;
            a4 = 0.418;

            athsched = 1;
            aexsched = 1;
            arthmn = 0.1; arthmx = 1.5;
            arexmn = 1.0; arexmx = 10.0;
            arthd = arth = .4;
            arexit = arexitd = 3.0;

            u0mt = 1500.0; u0mr = 4500.0;
            altmt = 60000.0; altmr = 100000.0;

            u0min = 0.0; u0max = u0mt;
            altmin = 0.0; altmax = altmt;
            thrmin = 30; thrmax = 100;
            etmin = .5; etmax = 1.0;
            cprmin = 1.0; cprmax = 50.0;
            bypmin = 0.0; bypmax = 10.0;
            fprmin = 1.0; fprmax = 2.0;
            t4min = 1000.0; t4max = 3200.0;
            t7min = 1000.0; t7max = 4000.0;
            a8min = 0.1; a8max = 0.4;
            a2min = .001; a2max = 50.0;
            pt4max = 1.0;
            diamin = Math.Sqrt(4.0 * a2min / 3.14159);
            diamax = Math.Sqrt(4.0 * a2max / 3.14159);
            pmax = 6000.0; tmin = -300.0 + tref; tmax = 600.0 + tref;
            vmn1 = u0min; vmx1 = u0max;
            vmn2 = altmin; vmx2 = altmax;
            vmn3 = thrmin; vmx3 = thrmax;
            vmn4 = arexmn; vmx4 = arexmx;

            weight = 1000.0;
            minlt = 1; dinlt = 170.2; tinlt = 900.0;
            mfan = 2; dfan = 293.02; tfan = 1500.0;
            mcomp = 2; dcomp = 293.02; tcomp = 1500.0;
            mburner = 4; dburner = 515.2; tburner = 2500.0;
            mturbin = 4; dturbin = 515.2; tturbin = 2500.0;
            mnozl = 3; dnozl = 515.2; tnozl = 2500.0;
            mnozr = 5; dnozr = 515.2; tnozr = 4500.0;

            comboBoxFuelType.Text = "Jet - A"; fueltype = 0;
            comboBoxEngineType.Text = "ТРД"; entype = 0;
            comboBoxInputType.Text = "Расчет по Скорости(V) и Высоте(H)"; inptype = 0;
            pt2flag = 1; // Чтобы всегда брать вводимый коэффициент восстановления давления в ВУ
            diameng = 1.595;
            wtflag = 0; // Для веса, только надо указать Плотность и температуру плавления материалов (сейчас не считает вообще)
            MOFCarbon = 0.9;
            MOFHydrogen = 0.08;
            MOFOxygen = 0.02;


            textBoxEps.Visible = false;



            comPute(); // Рассчитать
            pushData(); // Записать значения в ячейки
            pushExtraData(); // Записать значения в ячейки
            firstPageDesign(); // Стартовый дизайн первой страницы
        }
        #endregion


        #region First Page Design
        private void firstPageDesign()
        {
            Bitmap btmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = btmp;
            Graphics gr = Graphics.FromImage(pictureBox1.Image);

            int i = 0;
            int x = 9; int y = 9; // старт 9 9
            int interval = 27; // 27 интервал
            int dimension = 11; // размерность через 11
            int index = 8; // поправка на индекс
            int intervalBetweenColumns = 130;
            int startPosColumns = 200;


            #region Input Data
            // Параметры полета
            gr.DrawString("Параметры полета", new Font("Verdana", 11, FontStyle.Italic), new SolidBrush(Color.DodgerBlue), x - 3, y + interval * i);
            i++;

            // Диаметер двигателя (м)
            gr.DrawString("D", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("вх", new Font("Verdana", 8, FontStyle.Regular), brush, x + index + 4, y + interval * i + index);
            gr.DrawString(", м", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 16, y + interval * i);
            textBoxDiameter.Location = new Point(x + 70, y + interval * i + 2);
            i++;

            checkColor(0, 2);
            // Число Маха
            gr.DrawString("M", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i);
            textBoxMachNumber.Location = new Point(x + 70, y + interval * i + 2);
            i++;
            checkColor(1, 3);
            // Скорость полета (км/ч)
            gr.DrawString("V", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString(", км/ч", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension, y + interval * i);
            textBoxVFlight.Location = new Point(x + 70, y + interval * i + 2);
            i++;
            checkColor(2, 3);
            // Высота полета (км)
            gr.DrawString("H", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString(", км", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension, y + interval * i);
            textBoxAltitude.Location = new Point(x + 70, y + interval * i + 2);
            i++;
            checkColor(0, 1);
            // Давление невозмущенного потока (кПа)
            gr.DrawString("P", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("0", new Font("Verdana", 7, FontStyle.Italic), brush, x + index, y + interval * i + index);
            gr.DrawString(", кПа", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 4, y + interval * i);
            textBoxP0.Location = new Point(x + 70, y + interval * i + 2);
            i++;
            checkColor(0, 1);
            // Температура невозмущенного потока (К)
            gr.DrawString("T", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("0", new Font("Verdana", 7, FontStyle.Italic), brush, x + index, y + interval * i + index);
            gr.DrawString(", К", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 4, y + interval * i);
            textBoxT0.Location = new Point(x + 70, y + interval * i + 2);
            i++;
            brush.Color = Color.White;
            // РУД (%)
            gr.DrawString("РУД", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i);
            gr.DrawString(", %", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 19, y + interval * i);
            textBoxDrossel.Location = new Point(x + 70, y + interval * i + 2);
            i += 3;
            #endregion

            #region Fuel
            // Топливо
            gr.DrawString("Топливо", new Font("Verdana", 11, FontStyle.Italic), new SolidBrush(Color.Yellow), x + 36, y + interval * i);

            if (fueltype != 2)
                brush.Color = Color.Gray;
            else
                brush.Color = Color.White;
            // Низшая теплота сгорания
            gr.DrawString("Hu", new Font("Verdana", 11, FontStyle.Italic), brush, x + 150, y + interval * i + 1);
            gr.DrawString(", кДж/кг", new Font("Verdana", 11, FontStyle.Regular), brush, x + 150 + dimension + 11, y + interval * i + 1);

            brush.Color = Color.White;
            i++;
            textBoxHu.Location = new Point(x + 150, y + interval * i);
            textBoxHu.Width = 98;

            // Выбор топлива
            comboBoxFuelType.Location = new Point(x + 6, y + interval * i);
            comboBoxFuelType.Width = 136;
            i++;

            if (fueltype < 3)
                brush.Color = Color.Gray;
            else
                brush.Color = Color.White;
            // Массовая доля углерода
            gr.DrawString("\u03C9 (C)", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            textBoxMOF_C.Location = new Point(x + 70, y + interval * i + 2);
            i++;

            // Массовая доля углерода
            gr.DrawString("\u03C9 (H)", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            textBoxMOF_H.Location = new Point(x + 70, y + interval * i + 2);
            i++;

            // Массовая доля кислорода
            gr.DrawString("\u03C9 (O)", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            textBoxMOF_O.Location = new Point(x + 70, y + interval * i + 2);
            i += 3;
            brush.Color = Color.White;
            #endregion

            /*
            #region Eps
            // Точность
            gr.DrawString("\u03B5", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i - 5);
            textBoxEps.Location = new Point(x + 70, y + interval * i + 2 - 5);
            i++;
            #endregion
            */

            #region Inlet
            // ВУ
            i = 0;
            x = 245;
            gr.DrawString("ВУ", new Font("Verdana", 11, FontStyle.Italic), brushInlet, x, y + interval * i);
            i++;
            x = startPosColumns;
            index = 12;
            // Коэффициент посстановления давления в ВУ
            gr.DrawString("\u03C3", new Font("Times New Roman", 16, FontStyle.Regular), brush, x, y + interval * i - 6);
            gr.DrawString("вх", new Font("Verdana", 8, FontStyle.Regular), brush, x + index, y + interval * i - 6 + index);
            textBoxSigmaInlet.Location = new Point(x + 38, y + interval * i + 2);
            #endregion

            #region Fan
            if (entype <= 1)
            {
                brushFan.Color = Color.Gray;
                brush.Color = Color.Gray;
            }
            else
            {
                brushFan.Color = Color.GreenYellow;
                brush.Color = Color.White;
            }
            // Вентилятор
            i = 0;
            x = 343;
            gr.DrawString("Вентилятор", new Font("Verdana", 11, FontStyle.Italic), brushFan, x, y + interval * i);
            i++;
            x = startPosColumns + intervalBetweenColumns;
            index = 12;
            // Степень повышения давления вентилятора
            gr.DrawString("\u03C0", new Font("Times New Roman", 16, FontStyle.Italic), brush, x + 8, y + interval * i - 6);
            gr.DrawString("в", new Font("Verdana", 8, FontStyle.Regular), brush, x + 8 + index, y + interval * i - 6 + index);
            textBoxPiFan.Location = new Point(x + 38, y + interval * i + 2);
            i++;

            // Степень двухконтурности
            gr.DrawString("m", new Font("Verdana", 11, FontStyle.Italic), brush, x + 11, y + interval * i - 2);
            textBox_m.Location = new Point(x + 38, y + interval * i + 2);
            i++;

            // КПД вентилятора
            gr.DrawString("\u03B7", new Font("Times New Roman", 16, FontStyle.Regular), brush, x + 10, y + interval * i - 6);
            gr.DrawString("в", new Font("Verdana", 8, FontStyle.Regular), brush, x + 11 + index, y + interval * i - 6 + index);
            textBoxEtaFan.Location = new Point(x + 38, y + interval * i + 2);
            i++;
            brush.Color = Color.White;
            #endregion

            #region Compressor
            // Компрессор
            i = 0;
            x = 472;
            gr.DrawString("Компрессор", new Font("Verdana", 11, FontStyle.Italic), brushComp, x, y + interval * i);
            i++;
            x = startPosColumns + intervalBetweenColumns * 2;
            index = 12;
            // Степень повышения давления копрессора
            gr.DrawString("\u03C0", new Font("Times New Roman", 16, FontStyle.Italic), brush, x + 8, y + interval * i - 6);
            gr.DrawString("к", new Font("Verdana", 8, FontStyle.Regular), brush, x + 8 + index, y + interval * i - 6 + index);
            textBoxPiComp.Location = new Point(x + 38, y + interval * i + 2);
            i++;

            // КПД компрессора
            gr.DrawString("\u03B7", new Font("Times New Roman", 16, FontStyle.Regular), brush, x + 10, y + interval * i - 6);
            gr.DrawString("к", new Font("Verdana", 8, FontStyle.Regular), brush, x + 11 + index, y + interval * i - 6 + index);
            textBoxEtaComp.Location = new Point(x + 38, y + interval * i + 2);
            i++;
            #endregion

            #region Burner
            // КС
            i = 0;
            x = 628;
            gr.DrawString("КС", new Font("Verdana", 11, FontStyle.Italic), brushBurner, x, y + interval * i);
            i++;
            x = startPosColumns + intervalBetweenColumns * 3;
            index = 8;

            // Степень повышения давления копрессора
            gr.DrawString("T", new Font("Verdana", 11, FontStyle.Italic), brush, x - 8, y + interval * i);
            gr.DrawString("4", new Font("Verdana", 7, FontStyle.Italic), brush, x - 8 + index, y + interval * i + index);
            gr.DrawString(", К", new Font("Verdana", 11, FontStyle.Regular), brush, x - 8 + dimension + 4, y + interval * i);
            textBoxT4.Location = new Point(x + 38, y + interval * i + 2);
            i++;
            index = 12;

            // Коэффициент посстановления давления в КС
            gr.DrawString("\u03C3", new Font("Times New Roman", 16, FontStyle.Regular), brush, x - 8, y + interval * i - 6);
            gr.DrawString("кс", new Font("Verdana", 8, FontStyle.Regular), brush, x - 8 + index, y + interval * i - 6 + index);
            textBoxSigmaBurner.Location = new Point(x + 38, y + interval * i + 2);
            i++;

            // КПД компрессора
            gr.DrawString("\u03B7", new Font("Times New Roman", 16, FontStyle.Regular), brush, x - 8 + 1, y + interval * i - 7);
            gr.DrawString("г", new Font("Verdana", 8, FontStyle.Regular), brush, x - 8 + 2 + index, y + interval * i - 7 + index);
            textBoxEtaBurner.Location = new Point(x + 38, y + interval * i + 2);
            i++;
            #endregion

            #region Tubine
            // Турбина
            i = 0;
            x = 746;
            gr.DrawString("Турбина", new Font("Verdana", 11, FontStyle.Italic), brushTurbine, x, y + interval * i);
            i++;
            x = startPosColumns + intervalBetweenColumns * 4;
            index = 12;
            // КПД компрессора
            gr.DrawString("\u03B7", new Font("Times New Roman", 16, FontStyle.Regular), brush, x + 10, y + interval * i - 6);
            gr.DrawString("т", new Font("Verdana", 8, FontStyle.Regular), brush, x + 12 + index, y + interval * i - 6 + index);
            textBoxEtaTurbine.Location = new Point(x + 38, y + interval * i + 2);
            i++;
            #endregion

            #region Nozzle
            // Сопло
            i = 0;
            x = 885;
            gr.DrawString("Сопло", new Font("Verdana", 11, FontStyle.Italic), brushNozzle, x, y + interval * i);
            i++;
            x = startPosColumns + intervalBetweenColumns * 5;
            index = 12;

            // Температура ФК
            if (entype != 1)
                brush.Color = Color.Gray;
            else
                brush.Color = Color.White;
            gr.DrawString("T", new Font("Verdana", 11, FontStyle.Italic), brush, x - 8, y + interval * i);
            gr.DrawString("7", new Font("Verdana", 7, FontStyle.Italic), brush, x - 12 + index, y + interval * i + index - 4);
            gr.DrawString(", К", new Font("Verdana", 11, FontStyle.Regular), brush, x - 8 + dimension + 4, y + interval * i);
            textBoxAfterBurner.Location = new Point(x + 38, y + interval * i + 2);
            i++;
            brush.Color = Color.White;
            // Коэффициент скорости сопла
            gr.DrawString("\u03C6", new Font("Times New Roman", 16, FontStyle.Regular), brush, x - 8, y + interval * i - 7);
            gr.DrawString("с", new Font("Verdana", 8, FontStyle.Regular), brush, x - 6 + index, y + interval * i - 5 + index);
            textBoxFiNozzle.Location = new Point(x + 38, y + interval * i + 2);
            i++;

            #endregion

            #region Results
            // Прямоугольники
            i = 0;
            x = 548 - 50 - 1; y = 350 - 150 + 89;
            index = 12;
            gr.DrawRectangle(new Pen(Color.White), x, y, 447 + 50 + 1, 284 - 150 + 8);
            gr.DrawRectangle(new Pen(Color.White), x, y - 20, 447 + 50 + 1, 284 + 20 - 150 + 8 + 53);
            x += 1;

            gr.DrawString("Итоги расчета", new Font("Verdana", 11, FontStyle.Italic), brush, x + 166 + 25, y + interval * i - 20);

            x += 9; y += 9;

            // Тяга
            gr.DrawString("R", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString(", Н", new Font("Verdana", 11, FontStyle.Regular), brush, x - 6 + 8 + dimension, y + interval * i);
            textBoxThrust.Location = new Point(x + 56, y + interval * i + 1);
            i++;

            // Полная тяга (тяга сопла)
            gr.DrawString("R", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("с", new Font("Verdana", 8, FontStyle.Regular), brush, x + index, y + interval * i - 6 + index);
            gr.DrawString(", Н", new Font("Verdana", 11, FontStyle.Regular), brush, x + 8 + dimension, y + interval * i);
            textBoxGrossThrust.Location = new Point(x + 56, y + interval * i + 1);
            i++;

            // Лобовая сила
            gr.DrawString("R", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("л", new Font("Verdana", 8, FontStyle.Regular), brush, x + index, y + interval * i - 6 + index);
            gr.DrawString(", Н", new Font("Verdana", 11, FontStyle.Regular), brush, x + 8 + dimension, y + interval * i);
            textBoxRamDrag.Location = new Point(x + 56, y + interval * i + 1);
            i++;

            // Скоростной напор
            gr.DrawString("q", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i - 2);
            gr.DrawString("0", new Font("Verdana", 7, FontStyle.Italic), brush, x - 3 + index, y + interval * i - 6 + index);
            gr.DrawString(", Па", new Font("Verdana", 11, FontStyle.Regular), brush, x + 4 + dimension, y + interval * i);
            textBox_q0.Location = new Point(x + 56, y + interval * i + 1);
            i++;

            // Термический КПД
            gr.DrawString("\u03B7", new Font("Times New Roman", 16, FontStyle.Regular), brush, x - 2, y + interval * i - 6);
            gr.DrawString("t", new Font("Verdana", 8, FontStyle.Italic), brush, x + 1 + index, y + interval * i - 6 + index);
            textBoxThermalEff.Location = new Point(x + 56, y + interval * i + 1);
            i++;

            x += 145 - 3; i = 0;

            // Удельная тяга
            gr.DrawString("R", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("уд", new Font("Verdana", 8, FontStyle.Regular), brush, x + index, y + interval * i - 6 + index);
            gr.DrawString(", м/с", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 6 + 12, y + interval * i);
            textBoxThrustUdel.Location = new Point(x + 70 + 39, y + interval * i + 1);
            i++;

            // Удельный расход топлива
            gr.DrawString("c", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i - 2);
            gr.DrawString("уд", new Font("Verdana", 8, FontStyle.Regular), brush, x - 4 + 1 + index, y + interval * i - 6 + index);
            gr.DrawString(", кг/(Н⋅ч)", new Font("Verdana", 11, FontStyle.Regular), brush, x + 14 + dimension, y + interval * i);
            textBoxTSFC.Location = new Point(x + 70 + 39, y + interval * i + 1);
            i++;

            // Расход топлива
            gr.DrawString("G", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("т", new Font("Verdana", 8, FontStyle.Regular), brush, x + index, y + interval * i - 6 + index);
            gr.DrawString(", кг/ч", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 6, y + interval * i);
            textBoxGFuel.Location = new Point(x + 70 + 39, y + interval * i + 1);
            i++;

            // Расход воздуха
            gr.DrawString("G", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i - 1);
            gr.DrawString("в", new Font("Verdana", 8, FontStyle.Regular), brush, x + 1 + index, y + interval * i - 6 + index);
            gr.DrawString("1", new Font("Verdana", 6, FontStyle.Regular), brush, x + 1 + index + 8, y + interval * i - 5 + index + 4);
            gr.DrawString(", кг/с", new Font("Verdana", 11, FontStyle.Regular), brush, x + 8 + 8 + dimension, y + interval * i);
            textBoxEAir.Location = new Point(x + 70 + 39, y + interval * i + 1);
            i++;

            // Относительный расход топлива
            gr.DrawString("G", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("т", new Font("Verdana", 8, FontStyle.Regular), brush, x + index, y + interval * i - 6 + index);
            gr.DrawString("/G", new Font("Verdana", 11, FontStyle.Italic), brush, x + 20, y + interval * i);
            gr.DrawString("в", new Font("Verdana", 8, FontStyle.Regular), brush, x + 26 + index, y + interval * i - 6 + index);
            gr.DrawString("1", new Font("Verdana", 6, FontStyle.Regular), brush, x + 26 + 8 + index, y + interval * i - 6 + index + 4);
            textBoxFuelAir.Location = new Point(x + 70 + 39, y + interval * i + 1);
            i++;

            x += 195; i = 0;

            index = 8;
            // Диаметр сопла
            gr.DrawString("D", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("с", new Font("Verdana", 8, FontStyle.Regular), brush, x + index + 3, y + interval * i + index - 1);
            gr.DrawString(", м", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 8, y + interval * i);
            textBoxDiameterNozzle.Location = new Point(x + 70, y + interval * i + 1);
            i++;

            // Скорость истечения из сопла
            gr.DrawString("V", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("с", new Font("Verdana", 8, FontStyle.Regular), brush, x + index + 4 - 1, y + interval * i + index - 1);
            gr.DrawString(", м/с", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 8, y + interval * i);
            textBoxVExit.Location = new Point(x + 70, y + interval * i + 1);
            i++;

            // Статическая температура на срезе сопла
            gr.DrawString("T", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("с", new Font("Verdana", 8, FontStyle.Regular), brush, x + index + 4 - 1, y + interval * i + index - 1);
            gr.DrawString(", К", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 8, y + interval * i);
            textBoxTNozzle.Location = new Point(x + 70, y + interval * i + 1);
            i++;

            // Статическое давление на срезе сопла
            gr.DrawString("P", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("с", new Font("Verdana", 8, FontStyle.Regular), brush, x + index + 4 - 1, y + interval * i + index - 1);
            gr.DrawString(", кПа", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 8, y + interval * i);
            textBoxPNozzle.Location = new Point(x + 70, y + interval * i + 1);
            i++;

            // Статическое давление на срезе вентилятора
            if (entype <= 1)
            {
                brush.Color = Color.Gray;
                textBoxPFan.Text = "";
            }
            else
                brush.Color = Color.White;
            gr.DrawString("P", new Font("Verdana", 11, FontStyle.Italic), brush, x, y + interval * i);
            gr.DrawString("в", new Font("Verdana", 8, FontStyle.Regular), brush, x + index + 4 - 1, y + interval * i + index - 1);
            gr.DrawString(", кПа", new Font("Verdana", 11, FontStyle.Regular), brush, x + dimension + 8, y + interval * i);
            textBoxPFan.Location = new Point(x + 70, y + interval * i + 1);
            i += 2;
            brush.Color = Color.White;
            #endregion

            i = 12;
            x = 323; y = -75;
            interval = 27; // 27 интервал
            dimension = 11; // размерность через 11
            index = 8; // поправка на индекс

            #region P, T Total
            if (!hideTotalParameters)
            {

                textBoxTt1.Visible = true;
                textBoxTt2.Visible = true;
                textBoxTt3.Visible = true;
                textBoxTt4.Visible = true;
                textBoxTt5.Visible = true;
                textBoxTt6.Visible = true;
                textBoxTt7.Visible = true;
                textBoxTt8.Visible = true;

                textBoxPt1.Visible = true;
                textBoxPt2.Visible = true;
                textBoxPt3.Visible = true;
                textBoxPt4.Visible = true;
                textBoxPt5.Visible = true;
                textBoxPt6.Visible = true;
                textBoxPt7.Visible = true;
                textBoxPt8.Visible = true;

                // Параметры торможения
                //gr.DrawString("Параметры торможения", new Font("Verdana", 11, FontStyle.Italic), new SolidBrush(Color.DodgerBlue), x + 80, y + interval * i);
                //i++;

                // P первый столбец
                gr.DrawString("P", new Font("Verdana", 11, FontStyle.Italic), brush, x + 50 - 20, y + interval * i);
                gr.DrawString("*", new Font("Verdana", 7, FontStyle.Regular), brush, x + 50 - 20 + index + 4 - 1, y + interval * i - index + 4 + 4);
                gr.DrawString(", кПа", new Font("Verdana", 11, FontStyle.Regular), brush, x + 50 - 20 + dimension, y + interval * i);
                // T первый столбец
                gr.DrawString("T", new Font("Verdana", 11, FontStyle.Italic), brush, x + 135 - 19, y + interval * i);
                gr.DrawString("*", new Font("Verdana", 7, FontStyle.Regular), brush, x + 135 - 17 + index + 4 - 1, y + interval * i - index + 4 + 4);
                gr.DrawString(", К", new Font("Verdana", 11, FontStyle.Regular), brush, x + 135 - 19 + dimension, y + interval * i);
                // P второй столбец
                //gr.DrawString("P", new Font("Verdana", 11, FontStyle.Italic), brush, x + 210 - 5, y + interval * i);
                //gr.DrawString("*", new Font("Verdana", 7, FontStyle.Regular), brush, x + 210 - 5 + index + 4 - 1, y + interval * i - index + 4 + 4);
                //gr.DrawString(", кПа", new Font("Verdana", 11, FontStyle.Regular), brush, x + 210 - 5 + dimension, y + interval * i);
                // T второй столбец
                //gr.DrawString("T", new Font("Verdana", 11, FontStyle.Italic), brush, x + 285, y + interval * i);
                //gr.DrawString("*", new Font("Verdana", 7, FontStyle.Regular), brush, x + 287 + index + 4 - 1, y + interval * i - index + 4 + 4);
                //gr.DrawString(", К", new Font("Verdana", 11, FontStyle.Regular), brush, x + 285 + dimension, y + interval * i);
                i++;

                // Первая строка
                gr.DrawString("1:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt1.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt1.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Вторая строка
                gr.DrawString("2:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt2.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt2.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Третья строка
                gr.DrawString("3:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt3.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt3.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Четвертая строка
                gr.DrawString("4:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt4.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt4.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Пятая строка
                gr.DrawString("5:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt5.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt5.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Шестая строка
                gr.DrawString("6:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt6.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt6.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Седьмая строка
                gr.DrawString("7:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt7.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt7.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
                // Восьмая строка
                gr.DrawString("8:", new Font("Verdana", 11, FontStyle.Regular), brush, x, y + interval * i - 7);
                textBoxPt8.Location = new Point(x + 24, y + interval * i - 4);
                textBoxTt8.Location = new Point(x + 24 + 75, y + interval * i - 4);
                i++;
            }
            else
            {
                textBoxTt1.Visible = false;
                textBoxTt2.Visible = false;
                textBoxTt3.Visible = false;
                textBoxTt4.Visible = false;
                textBoxTt5.Visible = false;
                textBoxTt6.Visible = false;
                textBoxTt7.Visible = false;
                textBoxTt8.Visible = false;

                textBoxPt1.Visible = false;
                textBoxPt2.Visible = false;
                textBoxPt3.Visible = false;
                textBoxPt4.Visible = false;
                textBoxPt5.Visible = false;
                textBoxPt6.Visible = false;
                textBoxPt7.Visible = false;
                textBoxPt8.Visible = false;
            }
            #endregion

            gr.Dispose();
            GC.Collect();
        }
        #endregion

        #region Second Page Design
        private void secondPageDesign(Graphics gr)
        {
            OX = pictureBox3.Width / 2;
            OY = pictureBox3.Height / 2;

            inletLength = 5 * stageLength;
            burnerLength = 5 * stageLength;
            nozzleLength = 4 * stageLength;
            afterBurnerLength = 8 * stageLength;
            fanLength = 4 * stageLength;
            turbineLength = (stageLength + 4) * (nturb + 3);
            fullLength = 1;
            compLength = 1;


            int diameterCore = 100;
            int diameterFan = (int)(diameterCore + diameterCore * byprat / 4.5);
            int diameterNozzle = (int)((Math.Sqrt(4.0 * a8 / Math.PI) / (Math.Sqrt(4.0 * acore / Math.PI))) * diameterCore);
            int diam = diameterCore;


            #region Determine Full Engine Length
            if (entype == 0) // ТРД
            {
                fullLength = inletLength + burnerLength + nozzleLength +
                    (stageLength + 4) * (ncomp + 1) + turbineLength;
            }
            if (entype == 1) // ТРДФ
            {
                fullLength = inletLength + burnerLength + nozzleLength +
                    (stageLength + 4) * (ncomp + 1) + turbineLength + afterBurnerLength;
            }
            if (entype == 2) // ТРДД
            {
                fullLength = inletLength + burnerLength + nozzleLength +
                    (stageLength + 3) * ncomp + turbineLength + fanLength;

                diam = diameterFan;

                //gr.DrawRectangle(new Pen(Color.White), OX - fullLength / 2, OY - diameterFan / 2, fullLength, diameterFan); // fan
            }
            #endregion

            iX = OX - fullLength / 2;
            int iY = OY;

            // Сечение 0
            gr.DrawLine(new Pen(Color.Gray), new Point(iX - 100, 0), new Point(iX - 100, 2 * iY));
            gr.DrawString("0", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX - 100 + 2, 2 * iY - 20);

            #region Inlet
            // Верхняя часть ВУ
            gr.FillEllipse(brushInlet, iX + 1, iY - (diam / 2 + 10), 15, 15);
            PointF[] inletPoints = new PointF[4]
            {
                    new Point(iX + 6, iY - (diam / 2 + 10)), new Point(iX + 6, iY - (diam / 2 - 5)),
                    new Point(iX + inletLength, iY - (diam / 2 + 10)), new Point(iX + inletLength, iY - (diam / 2 + 20))
            };
            gr.FillPolygon(brushInlet, inletPoints);

            // Нижняя часть ВУ
            gr.FillEllipse(brushInlet, iX + 1, iY + (diam / 2 - 5), 15, 15);
            inletPoints = new PointF[4]
            {
                    new Point(iX + 6, iY + (diam / 2 + 10)), new Point(iX + 6, iY + (diam / 2 - 5)),
                    new Point(iX + inletLength, iY + (diam / 2 + 10)), new Point(iX + inletLength, iY + (diam / 2 + 20))
            };
            gr.FillPolygon(brushInlet, inletPoints);


            iX += inletLength;
            #endregion

            #region Compressor w/ Fan
            if (entype == 2)
            {
                compLength = (stageLength + 3) * ncomp;
                // Лопаты вентилятора
                gr.FillRectangle(brush, iX + stageLength, iY - (diam + 12) / 2 - 6, stageLength, diam + 24);
                gr.FillRectangle(brush, iX + stageLength + 20, iY - (diam + 12) / 2 - 6, stageLength, diam + 24);

                // Обтекатель
                gr.FillEllipse(brushFan, iX + 1, iY - 18, 36, 36);
                inletPoints = new PointF[4]
                {
                    new Point(iX + 18, iY + 18), new Point(iX + 18, iY - 18),
                    new Point(iX + fanLength, iY - 22), new Point(iX + fanLength, iY + 22)
                };
                gr.FillPolygon(brushFan, inletPoints);

                // Верхняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY - (diam / 2 + 19)),
                    new Point(iX, iY - (diam / 2 + 9)),
                    new Point(iX + fanLength + (ncomp * (stageLength + 3)), iY - (diam / 2 + 14)),
                    new Point(iX + fanLength + (ncomp * (stageLength + 3)), iY - (diam / 2 + 19))
                };
                gr.FillPolygon(brushFan, inletPoints);

                // Нижняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY + (diam / 2 + 20)),
                    new Point(iX, iY + (diam / 2 + 9)),
                    new Point(iX + fanLength + (ncomp * (stageLength + 3)), iY + (diam / 2 + 14)),
                    new Point(iX + fanLength + (ncomp * (stageLength + 3)), iY + (diam / 2 + 20))
                };
                gr.FillPolygon(brushFan, inletPoints);

                // Скругление у вентилятора
                gr.FillEllipse(brushFan, iX + fanLength + (ncomp * (stageLength + 3)) - 3, iY - (diam / 2 + 20), 7, 7);
                gr.FillEllipse(brushFan, iX + fanLength + (ncomp * (stageLength + 3)) - 3, iY + (diam / 2 + 13), 7, 7);

                // Сечение 1
                gr.DrawLine(new Pen(Color.Gray), new Point(iX, 0), new Point(iX, 2 * iY));
                gr.DrawString("1", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 2, 2 * iY - 20);

                iX += fanLength;
                diam = diameterCore;

                // Сечение 2
                gr.DrawLine(new Pen(Color.Gray), new Point(iX, 0), new Point(iX, 2 * iY));
                gr.DrawString("2", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 2, 2 * iY - 20);


                // Лопаты компрессора
                for (int h = 0; h < ncomp; h++)
                {
                    gr.FillRectangle(brush, iX + 5 + (h * 18), iY - (diam + 12) / 2 - 5, stageLength, diam + 23);
                }

                // Барабан компрессора
                inletPoints = new PointF[4]
                    {
                    new Point(iX, iY + 22), new Point(iX, iY - 22),
                    new Point(iX + ((stageLength + 4) * ncomp) + 5, iY - 30), new Point(iX + ((stageLength + 4) * ncomp) + 5, iY + 30)
                    };
                gr.FillPolygon(brushComp, inletPoints);

                // Верхняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY - (diam / 2 + 11)),
                    new Point(iX, iY - (diam / 2 + 3)),
                    new Point(iX + ((stageLength + 4) * ncomp) + 5, iY - (diam / 2 - 10)),
                    new Point(iX + ((stageLength + 4) * ncomp) + 5, iY - (diam / 2 + 11))
                };
                gr.FillPolygon(brushComp, inletPoints);

                // Нижняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY + (diam / 2 + 12)),
                    new Point(iX, iY + (diam / 2 + 3)),
                    new Point(iX + ((stageLength + 4) * ncomp) + 5, iY + (diam / 2 - 10)),
                    new Point(iX + ((stageLength + 4) * ncomp) + 5, iY + (diam / 2 + 12))
                };
                gr.FillPolygon(brushComp, inletPoints);

                // Скругление у корпуса
                gr.FillEllipse(brushComp, iX - 4, iY - (diameterCore / 2 + 11), 8, 8);
                gr.FillEllipse(brushComp, iX - 4, iY + (diameterCore / 2 + 3), 8, 8);

                iX += (stageLength + 4) * ncomp;
            }
            #endregion

            #region Compressor w/o Fan
            if (entype <= 1)
            {
                compLength = (stageLength + 4) * (ncomp + 1);
                // Лопаты компрессора
                for (int h = 1; h <= ncomp; h++)
                {
                    gr.FillRectangle(brush, iX + 5 + (h * 18), iY - (diam + 12) / 2 - 6, stageLength, diam + 24);
                }

                // Обтекатель
                gr.FillEllipse(brushComp, iX + 1, iY - 18, 36, 36);

                // Барабан компрессора
                inletPoints = new PointF[4]
                    {
                    new Point(iX + 18, iY + 18), new Point(iX + 18, iY - 18),
                    new Point(iX + ((stageLength + 4) * (ncomp + 1)) + 5, iY - 30), new Point(iX + ((stageLength + 4) * (ncomp + 1)) + 5, iY + 30)
                    };
                gr.FillPolygon(brushComp, inletPoints);

                // Верхняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY - (diam / 2 + 19)),
                    new Point(iX, iY - (diam / 2 + 10)),
                    new Point(iX + ((stageLength + 4) * (ncomp + 1)) + 5, iY - (diam / 2 - 10)),
                    new Point(iX + ((stageLength + 4) * (ncomp + 1)) + 5, iY - (diam / 2 + 19))
                };
                gr.FillPolygon(brushComp, inletPoints);

                // Верхняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY + (diam / 2 + 20)),
                    new Point(iX, iY + (diam / 2 + 10)),
                    new Point(iX + ((stageLength + 4) * (ncomp + 1)) + 5, iY + (diam / 2 - 10)),
                    new Point(iX + ((stageLength + 4) * (ncomp + 1)) + 5, iY + (diam / 2 + 20))
                };
                gr.FillPolygon(brushComp, inletPoints);

                // Сечение 2
                gr.DrawLine(new Pen(Color.Gray), new Point(iX, 0), new Point(iX, 2 * iY));
                gr.DrawString("2", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 2, 2 * iY - 20);

                iX += (stageLength + 4) * (ncomp + 1);
            }
            #endregion

            #region Burner
            iX += 5;
            // Центр (где вал внутри крутится)
            gr.FillRectangle(brushBurner, iX, iY - 15, burnerLength, 31);
            inletPoints = new PointF[3]
                {
                    new Point(iX + burnerLength / 2 + 5, iY),
                    new Point(iX + burnerLength, iY - 30),
                    new Point(iX + burnerLength, iY + 30)
                };
            gr.FillPolygon(brushBurner, inletPoints);


            if (entype <= 1)
            {
                // Верхняя часть корпуса
                inletPoints = new PointF[6]
                {
                    new Point(iX, iY - 50),
                    new Point(iX, iY - 69),
                    new Point(iX + burnerLength, iY - 69),
                    new Point(iX + burnerLength, iY - 50),
                    new Point(iX + burnerLength / 2 + 20, iY - 60),
                    new Point(iX + burnerLength / 2 - 20, iY - 60)
                };
                gr.FillPolygon(brushBurner, inletPoints);

                // Нижняя часть корпуса
                inletPoints = new PointF[6]
                {
                    new Point(iX, iY + 50),
                    new Point(iX, iY + 70),
                    new Point(iX + burnerLength, iY + 70),
                    new Point(iX + burnerLength, iY + 50),
                    new Point(iX + burnerLength / 2 + 20, iY + 60),
                    new Point(iX + burnerLength / 2 - 20, iY + 60)
                };
                gr.FillPolygon(brushBurner, inletPoints);

                // Дуги (форсунки)
                gr.DrawArc(new Pen(Color.White), iX + 5, iY - 45, 20, 20, 90, 180);
                gr.DrawArc(new Pen(Color.White), iX + 5, iY + 25, 20, 20, 90, 180);
            }
            else
            {
                // Верхняя часть корпуса
                inletPoints = new PointF[6]
                {
                    new Point(iX, iY - 44),
                    new Point(iX, iY - 61),
                    new Point(iX + burnerLength, iY - 61),
                    new Point(iX + burnerLength, iY - 44),
                    new Point(iX + burnerLength / 2 + 20, iY - 50),
                    new Point(iX + burnerLength / 2 - 20, iY - 50)
                };
                gr.FillPolygon(brushBurner, inletPoints);

                // Нижняя часть корпуса
                inletPoints = new PointF[6]
                {
                    new Point(iX, iY + 44),
                    new Point(iX, iY + 62),
                    new Point(iX + burnerLength, iY + 62),
                    new Point(iX + burnerLength, iY + 44),
                    new Point(iX + burnerLength / 2 + 20, iY + 50),
                    new Point(iX + burnerLength / 2 - 20, iY + 50)
                };
                gr.FillPolygon(brushBurner, inletPoints);

                // Дуги (форсунки)
                gr.DrawArc(new Pen(Color.White), iX + 5, iY - 45, 20, 20, 90, 180);
                gr.DrawArc(new Pen(Color.White), iX + 5, iY + 25, 20, 20, 90, 180);
            }

            // Сечение 3
            gr.DrawLine(new Pen(Color.Gray), new Point(iX, 0), new Point(iX, 2 * iY));
            gr.DrawString("3", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 2, 2 * iY - 20);

            iX += burnerLength;
            #endregion

            #region Turbine
            // Лопаты турбины
            int coreTurbineLength = 1;
            if (entype <= 1)
            {
                for (int h = 0; h < nturb; h++)
                {
                    gr.FillRectangle(brush, iX + 5 + (h * 18), iY - (diam + 12) / 2 - 5, stageLength, diam + 23);
                    coreTurbineLength = 5 + ((h + 1) * 18);
                }
                // Сечение 5
                gr.DrawLine(new Pen(Color.Gray), new Point(iX + coreTurbineLength - 4, 0), new Point(iX + coreTurbineLength - 4, 2 * iY));
                gr.DrawString("5", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + coreTurbineLength - 4 + 2, 2 * iY - 20);
            }
            else
            {
                for (int h = 0; h < nturb; h++)
                {
                    if (nturb - h == 1)
                    {
                        gr.FillRectangle(brush, iX + 15 + (h * 18), iY - (diam + 12) / 2 - 5, stageLength, diam + 23);
                        coreTurbineLength = 15 + ((h + 1) * 18);
                        // Сечение 5
                        gr.DrawLine(new Pen(Color.Gray), new Point(iX + 15 + (h * 18) - stageLength, 0), new Point(iX + 15 + (h * 18) - stageLength, 2 * iY));
                        gr.DrawString("5", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 15 + (h * 18) - stageLength + 2, 2 * iY - 20);
                    }
                    else
                    {
                        gr.FillRectangle(brush, iX + 5 + (h * 18), iY - (diam + 12) / 2 - 5, stageLength, diam + 23);
                        coreTurbineLength = 5 + ((h + 1) * 18);
                    }
                }
                // Сечение 6
                gr.DrawLine(new Pen(Color.Gray), new Point(iX + 15 + (nturb * 18) - 4, 0), new Point(iX + 15 + (nturb * 18) - 4, 2 * iY));
                gr.DrawString("6", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 15 + (nturb * 18) - 4 + 2, 2 * iY - 20);
            }
            // Обтекатель
            inletPoints = new PointF[3]
                {
                    new Point(iX, iY - 30),
                    new Point(iX + turbineLength, iY),
                    new Point(iX, iY + 30)
                };
            gr.FillPolygon(brushTurbine, inletPoints);

            // Корпус турбины
            if (entype <= 1)
            {
                // Верхняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY - 50),
                    new Point(iX, iY - 69),
                    new Point(iX + coreTurbineLength, iY - 69),
                    new Point(iX + coreTurbineLength, iY - 61)
                };
                gr.FillPolygon(brushTurbine, inletPoints);

                // Нижняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY + 50),
                    new Point(iX, iY + 70),
                    new Point(iX + coreTurbineLength, iY + 70),
                    new Point(iX + coreTurbineLength, iY + 61)
                };
                gr.FillPolygon(brushTurbine, inletPoints);
            }
            else
            {
                // Верхняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY - 44),
                    new Point(iX, iY - 61),
                    new Point(iX + coreTurbineLength, iY - 61),
                    new Point(iX + coreTurbineLength, iY - 54)
                };
                gr.FillPolygon(brushTurbine, inletPoints);

                // Нижняя часть корпуса
                inletPoints = new PointF[4]
                {
                    new Point(iX, iY + 44),
                    new Point(iX, iY + 62),
                    new Point(iX + coreTurbineLength, iY + 62),
                    new Point(iX + coreTurbineLength, iY + 54)
                };
                gr.FillPolygon(brushTurbine, inletPoints);
            }

            // Сечение 4
            gr.DrawLine(new Pen(Color.Gray), new Point(iX, 0), new Point(iX, 2 * iY));
            gr.DrawString("4", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 2, 2 * iY - 20);

            iX += coreTurbineLength;
            #endregion

            #region Nozzle
            if (entype == 0)
            {
                // Корпус
                gr.FillRectangle(brushNozzle, iX, iY - 69, nozzleLength, 8);
                gr.FillRectangle(brushNozzle, iX, iY + 61, nozzleLength, 9);

                iX += nozzleLength;

                // Верхняя створка
                inletPoints = new PointF[3]
                {
                    new Point(iX, iY - 61),
                    new Point(iX, iY - 69),
                    new Point(iX + (fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - inletLength), iY - diameterNozzle / 2 - 3),
                };
                gr.FillPolygon(brushNozzle, inletPoints);

                // Нижняя створка
                inletPoints = new PointF[3]
                {
                    new Point(iX, iY + 61),
                    new Point(iX, iY + 69),
                    new Point(iX + (fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - inletLength), iY + diameterNozzle / 2 + 3),
                };
                gr.FillPolygon(brushNozzle, inletPoints);

                // Сечение 8
                gr.DrawLine(new Pen(Color.Gray), new Point(iX + fullLength - nozzleLength - coreTurbineLength - burnerLength - compLength - inletLength - 2, 0),
                        new Point(iX + fullLength - nozzleLength - coreTurbineLength - burnerLength - compLength - inletLength - 2, 2 * iY));
                gr.DrawString("8", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - inletLength, 2 * iY - 20);
            }
            if (entype == 1)
            {
                // Корпус
                gr.FillRectangle(brushNozzle, iX, iY - 69, afterBurnerLength + nozzleLength / 2, 8);
                gr.FillRectangle(brushNozzle, iX, iY + 61, afterBurnerLength + nozzleLength / 2, 9);

                iX += afterBurnerLength + nozzleLength / 2;

                // Верхняя створка
                inletPoints = new PointF[3]
                {
                    new Point(iX, iY - 61),
                    new Point(iX, iY - 69),
                    new Point(iX + (fullLength - afterBurnerLength - (nozzleLength / 2) - coreTurbineLength -
                        burnerLength - compLength - inletLength), iY - diameterNozzle / 2 - 3),
                };
                gr.FillPolygon(brushNozzle, inletPoints);

                // Нижняя створка
                inletPoints = new PointF[3]
                {
                    new Point(iX, iY + 61),
                    new Point(iX, iY + 69),
                    new Point(iX + (fullLength - afterBurnerLength - (nozzleLength / 2) - coreTurbineLength -
                        burnerLength - compLength - inletLength), iY + diameterNozzle / 2 + 3),
                };
                gr.FillPolygon(brushNozzle, inletPoints);
                iX -= 25;
                // Треугольники (форсунки)
                gr.DrawLine(new Pen(Color.White), iX, iY - 30, iX + 10, iY - 30 - 10);
                gr.DrawLine(new Pen(Color.White), iX, iY - 30, iX + 10, iY - 30 + 10);
                gr.DrawLine(new Pen(Color.White), iX, iY + 30, iX + 10, iY + 30 - 10);
                gr.DrawLine(new Pen(Color.White), iX, iY + 30, iX + 10, iY + 30 + 10);


                // Сечение 7
                gr.DrawLine(new Pen(Color.Gray), new Point(iX + 25, 0), new Point(iX + 25, 2 * iY));
                gr.DrawString("7", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + 25 + 2, 2 * iY - 20);

                // Сечение 8
                gr.DrawLine(new Pen(Color.Gray), new Point(iX + fullLength - afterBurnerLength - (nozzleLength / 2) - coreTurbineLength -
                        burnerLength - compLength - inletLength + 23, 0),
                        new Point(iX + fullLength - afterBurnerLength - (nozzleLength / 2) - coreTurbineLength -
                        burnerLength - compLength - inletLength + 23, 2 * iY));
                gr.DrawString("8", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + fullLength - afterBurnerLength - (nozzleLength / 2) - coreTurbineLength -
                        burnerLength - compLength - inletLength + 23 + 2, 2 * iY - 20);
            }
            if (entype == 2)
            {
                // Корпус
                gr.FillRectangle(brushNozzle, iX, iY - 61, nozzleLength, 8);
                gr.FillRectangle(brushNozzle, iX, iY + 54, nozzleLength, 8);

                iX += nozzleLength;

                // Верхняя створка
                inletPoints = new PointF[3]
                {
                    new Point(iX, iY - 61),
                    new Point(iX, iY - 54),
                    new Point(iX + (fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - fanLength - inletLength), iY - diameterNozzle / 2 - 3),
                };
                gr.FillPolygon(brushNozzle, inletPoints);

                // Нижняя створка
                inletPoints = new PointF[3]
                {
                    new Point(iX, iY + 61),
                    new Point(iX, iY + 54),
                    new Point(iX + (fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - fanLength - inletLength), iY + diameterNozzle / 2 + 3),
                };
                gr.FillPolygon(brushNozzle, inletPoints);

                // Сечение 8
                gr.DrawLine(new Pen(Color.Gray), new Point(iX + fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - fanLength - inletLength - 2, 0),
                        new Point(iX + fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - fanLength - inletLength - 2, 2 * iY));
                gr.DrawString("8", new Font("Verdana", 11, FontStyle.Regular), new SolidBrush(Color.Gray), iX + fullLength - nozzleLength - coreTurbineLength -
                        burnerLength - compLength - fanLength - inletLength - 2 + 2, 2 * iY - 20);
            }
            #endregion

            // core
            //gr.DrawRectangle(new Pen(Color.White), OX - fullLength / 2, OY - diameterCore / 2, fullLength, diameterCore);

            // nozzle
            //gr.DrawRectangle(new Pen(Color.Red), OX - fullLength / 2, OY - diameterNozzle / 2, fullLength, diameterNozzle);

            #region Main Stream

            // Невозмущенный поток, ВУ
            iX = OX - fullLength / 2;
            if (iter == 0)
            {
                for (int h = 0; h < 3; h++)
                {
                    // freestream
                    gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX - 140, iY + (h * 18)), new Point(iX - 210, iY + (h * 18)));
                    gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX - 140, iY - (h * 18)), new Point(iX - 210, iY - (h * 18)));
                    // inlet
                    gr.DrawLine(new Pen(Color.Teal), new Point(iX, iY + (h * 18)), new Point(iX + inletLength / 3, iY + (h * 18) + 5));
                    gr.DrawLine(new Pen(Color.Teal), new Point(iX, iY - (h * 18)), new Point(iX + inletLength / 3, iY - (h * 18) - 5));
                }
            }
            if (iter == 1)
            {
                for (int h = 0; h < 3; h++)
                {
                    // freestream
                    gr.DrawLine(new Pen(Color.Cyan), new Point(iX - 70, iY + (h * 18)), new Point(iX - 140, iY + (h * 18)));
                    gr.DrawLine(new Pen(Color.Cyan), new Point(iX - 70, iY - (h * 18)), new Point(iX - 140, iY - (h * 18)));
                    // inlet
                    gr.DrawLine(new Pen(Color.Cyan), new Point(iX + inletLength / 3 + 5, iY + (h * 18) + 5), new Point(iX + 2 * inletLength / 3 + 5, iY + (h * 18) + 10));
                    gr.DrawLine(new Pen(Color.Cyan), new Point(iX + inletLength / 3 + 5, iY - (h * 18) - 5), new Point(iX + 2 * inletLength / 3 + 5, iY - (h * 18) - 10));
                }
            }
            if (iter == 2)
            {
                for (int h = 0; h < 3; h++)
                {
                    // freestream
                    gr.DrawLine(new Pen(Color.Teal), new Point(iX, iY + (h * 18)), new Point(iX - 70, iY + (h * 18)));
                    gr.DrawLine(new Pen(Color.Teal), new Point(iX, iY - (h * 18)), new Point(iX - 70, iY - (h * 18)));
                    // inlet
                    gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX + 2 * inletLength / 3, iY + (h * 18) + 10), new Point(iX + inletLength, iY + (h * 18) + 15));
                    gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX + 2 * inletLength / 3, iY - (h * 18) - 10), new Point(iX + inletLength, iY - (h * 18) - 15));
                }
            }
            // Компрессор
            iX += inletLength;

            int fl = 0;
            if (entype == 2)
                fl = fanLength + stageLength / 2 + 5;
            if (iter == 0)
            {
                gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX, iY + (2 * 20) + 12), new Point(iX + (fl + compLength) / 3, iY + (2 * 20) + 8));
                gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX, iY - (2 * 20) - 12), new Point(iX + (fl + compLength) / 3, iY - (2 * 20) - 8));
                gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX, iY + (1 * 20)), new Point(iX + (fl + compLength) / 3, iY + (1 * 20) + 8));
                gr.DrawLine(new Pen(Color.DodgerBlue), new Point(iX, iY - (1 * 20)), new Point(iX + (fl + compLength) / 3, iY - (1 * 20) - 8));
            }
            if (iter == 1)
            {
                gr.DrawLine(new Pen(Color.Cyan), new Point(iX + (fl + compLength) / 3, iY + (2 * 20) + 8), new Point(iX + 2 * (fl + compLength) / 3, iY + (2 * 20) + 4));
                gr.DrawLine(new Pen(Color.Cyan), new Point(iX + (fl + compLength) / 3, iY - (2 * 20) - 8), new Point(iX + 2 * (fl + compLength) / 3, iY - (2 * 20) - 4));
                gr.DrawLine(new Pen(Color.Cyan), new Point(iX + (fl + compLength) / 3, iY + (1 * 20) + 8), new Point(iX + 2 * (fl + compLength) / 3, iY + (1 * 20) + 10));
                gr.DrawLine(new Pen(Color.Cyan), new Point(iX + (fl + compLength) / 3, iY - (1 * 20) - 8), new Point(iX + 2 * (fl + compLength) / 3, iY - (1 * 20) - 10));
            }
            if (iter == 2)
            {
                gr.DrawLine(new Pen(Color.Teal), new Point(iX + 2 * (fl + compLength) / 3, iY + (2 * 20) + 4), new Point(iX + fl + compLength, iY + (2 * 20) - 2));
                gr.DrawLine(new Pen(Color.Teal), new Point(iX + 2 * (fl + compLength) / 3, iY - (2 * 20) - 4), new Point(iX + fl + compLength, iY - (2 * 20) + 2));
                gr.DrawLine(new Pen(Color.Teal), new Point(iX + 2 * (fl + compLength) / 3, iY + (1 * 20) + 10), new Point(iX + fl + compLength, iY + (1 * 20) + 12));
                gr.DrawLine(new Pen(Color.Teal), new Point(iX + 2 * (fl + compLength) / 3, iY - (1 * 20) - 10), new Point(iX + fl + compLength, iY - (1 * 20) - 12));
            }

            // КС
            iX += fl + compLength + 5;
            if (iter == 0)
            {
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY + 30), new Point(iX + (burnerLength) / 3, iY + 30 - 5));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY + 30 + 10), new Point(iX + (burnerLength) / 3, iY + 30 + 15));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY - 30), new Point(iX + (burnerLength) / 3, iY - 30 + 5));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY - 30 - 10), new Point(iX + (burnerLength) / 3, iY - 30 - 15));
            }
            if (iter == 1)
            {
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (burnerLength) / 3, iY + 30 - 5), new Point(iX + (2 * burnerLength) / 3, iY + 30 - 5));
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (burnerLength) / 3, iY + 30 + 10 + 5), new Point(iX + (2 * burnerLength) / 3, iY + 30 + 10 + 5));
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (burnerLength) / 3, iY - 30 + 5), new Point(iX + (2 * burnerLength) / 3, iY - 30 + 5));
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (burnerLength) / 3, iY - 30 - 10 - 5), new Point(iX + (2 * burnerLength) / 3, iY - 30 - 10 - 5));
            }
            if (iter == 2)
            {
                gr.DrawLine(new Pen(Color.Red), new Point(iX + (2 * burnerLength) / 3, iY + 30 - 5), new Point(iX + (burnerLength), iY + 30 + 3));
                gr.DrawLine(new Pen(Color.Red), new Point(iX + (2 * burnerLength) / 3, iY + 30 + 10 + 5), new Point(iX + (burnerLength), iY + 30 + 10));
                gr.DrawLine(new Pen(Color.Red), new Point(iX + (2 * burnerLength) / 3, iY - 30 + 5), new Point(iX + (burnerLength), iY - 30 - 3));
                gr.DrawLine(new Pen(Color.Red), new Point(iX + (2 * burnerLength) / 3, iY - 30 - 10 - 5), new Point(iX + (burnerLength), iY - 30 - 10));
            }
            // Сопло
            iX += burnerLength;
            int otcLength = fullLength - (inletLength + fl + compLength + 5 + burnerLength);
            if (iter == 0)
            {
                gr.DrawLine(new Pen(Color.Red), new Point(iX, iY + 30 + 3), new Point(iX + (otcLength) / 3, iY + 30 - 5));
                gr.DrawLine(new Pen(Color.Red), new Point(iX, iY + 30 + 10), new Point(iX + (otcLength) / 3, iY + 30 + 15));

                gr.DrawLine(new Pen(Color.Red), new Point(iX, iY - 30 - 3), new Point(iX + (otcLength) / 3, iY - 30 + 5));
                gr.DrawLine(new Pen(Color.Red), new Point(iX, iY - 30 - 10), new Point(iX + (otcLength) / 3, iY - 30 - 15));
            }
            if (iter == 1)
            {
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (otcLength) / 3, iY + 30 - 5), new Point(iX + (2 * otcLength) / 3, iY + 30 - 10));
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (otcLength) / 3, iY + 30 + 10 + 5), new Point(iX + (2 * otcLength) / 3, iY + 30 + 10 + 0));
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (otcLength) / 3, iY - 30 + 5), new Point(iX + (2 * otcLength) / 3, iY - 30 + 10));
                gr.DrawLine(new Pen(Color.Orange), new Point(iX + (otcLength) / 3, iY - 30 - 10 - 5), new Point(iX + (2 * otcLength) / 3, iY - 30 - 10 - 0));
            }
            if (iter == 2)
            {
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX + (2 * otcLength) / 3, iY + 30 - 10), new Point(iX + (otcLength), iY + 30 - 20));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX + (2 * otcLength) / 3, iY + 30 + 10), new Point(iX + (otcLength), iY + diameterNozzle / 3 + 5));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX + (2 * otcLength) / 3, iY - 30 + 10), new Point(iX + (otcLength), iY - 30 + 20));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX + (2 * otcLength) / 3, iY - 30 - 10), new Point(iX + (otcLength), iY - diameterNozzle / 3 - 5));
            }
            // За соплом
            iX += otcLength;
            if (iter == 0)
            {
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY + diameterNozzle / 3 + 5), new Point(iX + 70, iY + 30 + 10));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY + 30 - 20), new Point(iX + 70, iY + 30 - 15));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY - diameterNozzle / 3 - 5), new Point(iX + 70, iY - 30 - 10));
                gr.DrawLine(new Pen(Color.Yellow), new Point(iX, iY - 30 + 20), new Point(iX + 70, iY - 30 + 15));
            }
            if (iter == 1)
            {
                gr.DrawLine(new Pen(Color.Khaki), new Point(iX + 70, iY + 30 + 10), new Point(iX + 140, iY + 30 + 20));
                gr.DrawLine(new Pen(Color.Khaki), new Point(iX + 70, iY + 30 - 15), new Point(iX + 140, iY + 30 - 15));
                gr.DrawLine(new Pen(Color.Khaki), new Point(iX + 70, iY - 30 - 10), new Point(iX + 140, iY - 30 - 20));
                gr.DrawLine(new Pen(Color.Khaki), new Point(iX + 70, iY - 30 + 15), new Point(iX + 140, iY - 30 + 15));
            }
            if (iter == 2)
            {
                gr.DrawLine(new Pen(Color.Bisque), new Point(iX + 140, iY + 30 + 20), new Point(iX + 210, iY + 30 + 20));
                gr.DrawLine(new Pen(Color.Bisque), new Point(iX + 140, iY + 30 - 15), new Point(iX + 210, iY + 30 - 15));
                gr.DrawLine(new Pen(Color.Bisque), new Point(iX + 140, iY - 30 - 20), new Point(iX + 210, iY - 30 - 20));
                gr.DrawLine(new Pen(Color.Bisque), new Point(iX + 140, iY - 30 + 15), new Point(iX + 210, iY - 30 + 15));
            }
            #endregion

            #region Free Stream
            if (entype == 2)
            {
                for (int d = diameterCore; d < diameterFan; d += 30)
                {
                    for (int h = 0; h < pictureBox3.Width; h += 60)
                    {
                        if (iter == 0)
                        {
                            gr.DrawLine(new Pen(Color.White), new Point(h, OY - d / 2 - 14), new Point(h + 10, OY - d / 2 - 14));
                            gr.DrawLine(new Pen(Color.White), new Point(h, OY + d / 2 + 14), new Point(h + 10, OY + d / 2 + 14));
                        }
                        if (iter == 1)
                        {
                            gr.DrawLine(new Pen(Color.White), new Point(h + 20, OY - d / 2 - 14), new Point(h + 30, OY - d / 2 - 14));
                            gr.DrawLine(new Pen(Color.White), new Point(h + 20, OY + d / 2 + 14), new Point(h + 30, OY + d / 2 + 14));
                        }
                        if (iter == 2)
                        {
                            gr.DrawLine(new Pen(Color.White), new Point(h + 40, OY - d / 2 - 14), new Point(h + 50, OY - d / 2 - 14));
                            gr.DrawLine(new Pen(Color.White), new Point(h + 40, OY + d / 2 + 14), new Point(h + 50, OY + d / 2 + 14));
                        }
                    }
                }
            }
            #endregion
        }
        #endregion



        #region Misc Functions

        #region Check Color
        private void checkColor(int a, int b)
        {
            if ((inptype == a) || (inptype == b))
                brush.Color = Color.Gray;
            else
                brush.Color = Color.White;
        }
        #endregion

        #region Filter
        private double filter(double num)
        {
            return Math.Round(num, 3);
        }
        #endregion

        #region Timer Tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap btmp = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Image = btmp;
            Graphics gr = Graphics.FromImage(pictureBox3.Image);

            secondPageDesign(gr);
            iter++;
            if (iter == 3)
                iter = 0;
            GC.Collect();
        }
        #endregion

        #region Select 2-nd Page
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 1)
                timer1.Enabled = true;
            else
                timer1.Enabled = false;
        }
        #endregion

        #region Only Thrust And TSFC
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            hideTotalParameters = !hideTotalParameters;
            firstPageDesign();
        }
        #endregion

        #region Key Press
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | (e.KeyChar == Convert.ToChar(".")) | e.KeyChar == '\b') return;
            else if (e.KeyChar == (char)Keys.Return)
                calcAll();
            else
                e.Handled = true;
        }
        private void all_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void eps_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }
        #endregion

        #region Remove Dots
        private string removeDots(string str)
        {
            string text = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '.')
                    text += ',';
                else
                    text += str[i];
            }
            return text;
        }
        #endregion

        #region EPR
        private void enginePressureRatio()
        {
            if (pt[7] < ps0)
            {
                MessageBox.Show("Давление в сопле ниже атмосферного, расчет невоможен. Требуется увеличить температуру перед турбиной - T4");
            }
        }
        #endregion

        #endregion

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            calcAll();
        }

        private void calcAll()
        {
            if ((entype == 1) && (textBoxDiameterNozzle.Text == "*"))
            {
                MessageBox.Show("Мне даже не платят, а хотят чтобы окошко раскрывалось...       coder: vk.com/65k_na_moih_nogah");
            }

            try
            {
                getData();
            }
            catch
            {
                MessageBox.Show("Неверный формат ввода");
                return;
            }
            comPute();
            pushData();
            pushExtraData();
            firstPageDesign();

            enginePressureRatio();

            GC.Collect();
        }
    }
}


using System;

namespace DarkMode_2.Models
{
    public static class TimeConverter
    {
        #region 公共方法
        /// <summary>
        /// 计算日长
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns>日长</returns>
        /// <remarks>
        /// 注：日期最小为2000.1.1号
        /// </remarks>
        public static double GetDayLength(DateTime date, double longitude, double latitude)
        {
            double result = DayLen(date.Year, date.Month, date.Day, longitude, latitude, -35.0 / 60.0, 1);
            return result;
        }

        /// <summary>
        /// 计算日出日没时间
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns>日落日出时间</returns>
        /// <remarks>
        /// 注：日期最小为2000.1.1号
        /// </remarks>
        public static SunTimeResult GetSunTime(DateTime date, double longitude, double latitude)
        {
            double start = 0;
            double end = 0;
            SunRiset(date.Year, date.Month, date.Day, longitude, latitude, -35.0 / 60.0, 1, ref start, ref end);
            DateTime sunrise = ToLocalTime(date, start);
            DateTime sunset = ToLocalTime(date, end);
            return new SunTimeResult(sunrise, sunset);
        }
        #endregion

        #region 私有方法

        #region 时间转换
        private static DateTime ToLocalTime(DateTime time, double utTime)
        {
            int hour = Convert.ToInt32(Math.Floor(utTime));
            double temp = utTime - hour;
            hour += 8;//转换为东8区北京时间
            temp = temp * 60;
            int minute = Convert.ToInt32(Math.Floor(temp));
            try
            {
                return new DateTime(time.Year, time.Month, time.Day, hour, minute, 0);
            }
            catch
            {
                return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            }
        }
        #endregion

        #region 与日出日落时间相关计算
        private static double DayLen(int year, int month, int day, double lon, double lat,
            double altit, int upper_limb)
        {
            double d,  /* Days since 2000 Jan 0.0 (negative before) */
                obl_ecl,    /* Obliquity (inclination) of Earth\'s axis */
                //黄赤交角，在2000.0历元下国际规定为23度26分21.448秒，但有很小的时间演化。

                sr,         /* Solar distance, astronomical units */
                slon,       /* True solar longitude */
                sin_sdecl,  /* Sine of Sun\'s declination */
                //太阳赤纬的正弦值。
                cos_sdecl,  /* Cosine of Sun\'s declination */
                sradius,    /* Sun\'s apparent radius */
                t;          /* Diurnal arc */

            /* Compute d of 12h local mean solar time */
            d = Days_since_2000_Jan_0(year, month, day) + 0.5 - lon / 360.0;

            /* Compute obliquity of ecliptic (inclination of Earth\'s axis) */
            obl_ecl = 23.4393 - 3.563E-7 * d;
            //这个黄赤交角时变公式来历复杂，很大程度是经验性的，不必追究。

            /* Compute Sun\'s position */
            slon = 0.0;
            sr = 0.0;
            Sunpos(d, ref slon, ref sr);

            /* Compute sine and cosine of Sun\'s declination */
            sin_sdecl = Sind(obl_ecl) * Sind(slon);
            cos_sdecl = Math.Sqrt(1.0 - sin_sdecl * sin_sdecl);
            //用球面三角学公式计算太阳赤纬。

            /* Compute the Sun\'s apparent radius, degrees */
            sradius = 0.2666 / sr;
            //视半径，同前。

            /* Do correction to upper limb, if necessary */
            if (upper_limb != 0)
                altit -= sradius;

            /* Compute the diurnal arc that the Sun traverses to reach */
            /* the specified altitide altit: */
            //根据设定的地平高度判据计算周日弧长。
            double cost;
            cost = (Sind(altit) - Sind(lat) * sin_sdecl) /
                (Cosd(lat) * cos_sdecl);
            if (cost >= 1.0)
                t = 0.0;                      /* Sun always below altit */
            //极夜。
            else if (cost <= -1.0)
                t = 24.0;                     /* Sun always above altit */
            //极昼。
            else t = (2.0 / 15.0) * Acosd(cost); /* The diurnal arc, hours */
            //周日弧换算成小时计。
            return t;

        }

        private static void Sunpos(double d, ref double lon, ref double r)
        {
            double M,//太阳的平均近点角，从太阳观察到的地球（=从地球看到太阳的）距近日点（近地点）的角度。
                w, //近日点的平均黄道经度。
                e, //地球椭圆公转轨道离心率。
                E, //太阳的偏近点角。计算公式见下面。

                x, y,
                v;  //真近点角，太阳在任意时刻的真实近点角。


            M = Revolution(356.0470 + 0.9856002585 * d);//自变量的组成：2000.0时刻太阳黄经为356.0470度,此后每天约推进一度（360度/365天
            w = 282.9404 + 4.70935E-5 * d;//近日点的平均黄经。

            e = 0.016709 - 1.151E-9 * d;//地球公转椭圆轨道离心率的时间演化。以上公式和黄赤交角公式一样，不必深究。

            E = M + e * Radge * Sind(M) * (1.0 + e * Cosd(M));
            x = Cosd(E) - e;
            y = Math.Sqrt(1.0 - e * e) * Sind(E);
            r = Math.Sqrt(x * x + y * y);
            v = Atan2d(y, x);
            lon = v + w;
            if (lon >= 360.0)
                lon -= 360.0;
        }

        private static void Sun_RA_dec(double d, ref double RA, ref double dec, ref double r)
        {
            double lon, obl_ecl, x, y, z;
            lon = 0.0;

            Sunpos(d, ref lon, ref r);
            //计算太阳的黄道坐标。

            x = r * Cosd(lon);
            y = r * Sind(lon);
            //计算太阳的直角坐标。

            obl_ecl = 23.4393 - 3.563E-7 * d;
            //黄赤交角，同前。

            z = y * Sind(obl_ecl);
            y = y * Cosd(obl_ecl);
            //把太阳的黄道坐标转换成赤道坐标（暂改用直角坐标）。

            RA = Atan2d(y, x);
            dec = Atan2d(z, Math.Sqrt(x * x + y * y));
            //最后转成赤道坐标。显然太阳的位置是由黄道坐标方便地直接确定的，但必须转换到赤
            //道坐标里才能结合地球的自转确定我们需要的白昼长度。

        }
        /// <summary>
        /// 日出没时刻计算
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="altit"></param>
        /// <param name="upper_limb"></param>
        /// <param name="trise">日出时刻</param>
        /// <param name="tset">日没时刻</param>
        /// <returns>太阳有出没现象，返回0 极昼，返回+1 极夜，返回-1</returns>
        private static int SunRiset(int year, int month, int day, double lon, double lat,
            double altit, int upper_limb, ref double trise, ref double tset)
        {
            double d,  /* Days since 2000 Jan 0.0 (negative before) */
                //以历元2000.0起算的日数。

                sr,         /* Solar distance, astronomical units */
                //太阳距离，以天文单位计算（约1.5亿公里）。      

                sRA,        /* Sun\'s Right Ascension */
                //同前，太阳赤经。

                sdec,       /* Sun\'s declination */
                //太阳赤纬。

                sradius,    /* Sun\'s apparent radius */
                //太阳视半径，约16分（受日地距离、大气折射等诸多影响）

                t,          /* Diurnal arc */
                //周日弧，太阳一天在天上走过的弧长。

                tsouth,     /* Time when Sun is at south */
                sidtime;    /* Local sidereal time */
            //当地恒星时，即地球的真实自转周期。比平均太阳日（日常时间）长3分56秒。      

            int rc = 0; /* Return cde from function - usually 0 */

            /* Compute d of 12h local mean solar time */
            d = Days_since_2000_Jan_0(year, month, day) + 0.5 - lon / 360.0;
            //计算观测地当日中午时刻对应2000.0起算的日数。

            /* Compute local sideral time of this moment */
            sidtime = Revolution(GMST0(d) + 180.0 + lon);
            //计算同时刻的当地恒星时（以角度为单位）。以格林尼治为基准，用经度差校正。

            /* Compute Sun\'s RA + Decl at this moment */
            sRA = 0.0;
            sdec = 0.0;
            sr = 0.0;
            Sun_RA_dec(d, ref sRA, ref sdec, ref sr);
            //计算同时刻太阳赤经赤纬。

            /* Compute time when Sun is at south - in hours UT */
            tsouth = 12.0 - Rev180(sidtime - sRA) / 15.0;
            //计算太阳日的正午时刻，以世界时（格林尼治平太阳时）的小时计。

            /* Compute the Sun\'s apparent radius, degrees */
            sradius = 0.2666 / sr;
            //太阳视半径。0.2666是一天文单位处的太阳视半径（角度）。

            /* Do correction to upper limb, if necessary */
            if (upper_limb != 0)
                altit -= sradius;
            //如果要用上边缘，就要扣除一个视半径。

            /* Compute the diurnal arc that the Sun traverses to reach */
            //计算周日弧。直接利用球面三角公式。如果碰到极昼极夜问题，同前处理。
            /* the specified altitide altit: */

            double cost;
            cost = (Sind(altit) - Sind(lat) * Sind(sdec)) /
                (Cosd(lat) * Cosd(sdec));
            if (cost >= 1.0)
            {
                rc = -1;
                t = 0.0;
            }
            else
            {
                if (cost <= -1.0)
                {
                    rc = +1;
                    t = 12.0;      /* Sun always above altit */
                }
                else
                    t = Acosd(cost) / 15.0;   /* The diurnal arc, hours */
            }


            /* Store rise and set times - in hours UT */
            trise = tsouth - t;
            tset = tsouth + t;

            return rc;
        }
        #endregion

        #region 辅助函数
        /// <summary>
        /// 历元2000.0，即以2000年第一天开端为计日起始（天文学以第一天为0日而非1日）。
        /// 它与UT（就是世界时，格林尼治平均太阳时）1999年末重合。 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static long Days_since_2000_Jan_0(int y, int m, int d)
        {
            return (367L * (y) - ((7 * ((y) + (((m) + 9) / 12))) / 4) + ((275 * (m)) / 9) + (d) - 730530L);
        }

        private static double Revolution(double x)
        {
            return (x - 360.0 * Math.Floor(x * Inv360));
        }

        private static double Rev180(double x)
        {
            return (x - 360.0 * Math.Floor(x * Inv360 + 0.5));
        }

        private static double GMST0(double d)
        {
            double sidtim0;
            sidtim0 = Revolution((180.0 + 356.0470 + 282.9404) +
                (0.9856002585 + 4.70935E-5) * d);
            return sidtim0;
        }


        private static double Inv360 = 1.0 / 360.0;
        #endregion

        #region 度与弧度转换系数，为球面三角计算作准备
        private static double Sind(double x)
        {
            return Math.Sin(x * Degrad);
        }

        private static double Cosd(double x)
        {
            return Math.Cos(x * Degrad);
        }

        private static double Tand(double x)
        {
            return Math.Tan(x * Degrad);

        }

        private static double Atand(double x)
        {
            return Radge * Math.Atan(x);
        }

        private static double Asind(double x)
        {
            return Radge * Math.Asin(x);
        }

        private static double Acosd(double x)
        {
            return Radge * Math.Acos(x);
        }

        private static double Atan2d(double y, double x)
        {
            return Radge * Math.Atan2(y, x);

        }

        private static double Radge = 180.0 / Math.PI;
        private static double Degrad = Math.PI / 180.0;

        #endregion

        #endregion
    }

    /// <summary>
    /// 日出日落时间结果
    /// </summary>
    public class SunTimeResult
    {
        #region 构造与析构
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sunrise">日出时间</param>
        /// <param name="sunset">日落时间</param>
        public SunTimeResult(DateTime sunrise, DateTime sunset)
        {
            sunriseTime = sunrise;
            sunsetTime = sunset;
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 获取日出时间
        /// </summary>
        public DateTime SunriseTime
        {
            get
            {
                return sunriseTime;
            }
        }

        /// <summary>
        /// 获取日落时间
        /// </summary>
        public DateTime SunsetTime
        {
            get
            {
                return sunsetTime;
            }
        }
        #endregion


        #region 私成员
        private DateTime sunriseTime;//日出时间
        private DateTime sunsetTime;//日落时间
        #endregion
    }
}

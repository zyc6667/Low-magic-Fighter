using System;

namespace Low_magic_Fighter
{
    public static class Tool
    {
        //绝对值
        public static int GetAbs(int num)
        {
            if(num>=0) return num;
            else return -num;
        }

        public static double DiTui(double X1,int nCount) //1000题基础6.15
        {
            double Xn=X1;
            for(int i=0;i<nCount;i++)
            {
                Xn=Math.Acos(Math.Sin(Xn)/Xn); //Xn+1=arccos(sin(Xn)/Xn)
            }
            return Xn;
        }

        public static double CunKuan(double interestRate,int year) //1000题基础6.15
        {
            double multiple=1;
            for(int i=0;i<year;i++)
            {
                multiple*=1+interestRate;
            }
            return multiple;
        }
    }
}
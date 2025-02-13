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

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiyoS.Draw
{
    class RandomF
    {
        // n 生成随机数个数
        public int[] GenerateUniqueRandom(int minValue, int maxValue, int n)
        {
            // Random.Nex(1, 10) 只能产生到 9 的随机数，若要产生到 10 的随机数， maxValue 要加 1
            maxValue++;

            // Random.Nex(1, 10) 只能产生 9 个随机数，因此 n 不能大于 10 - 1
            if (n > maxValue - minValue)
                n = maxValue - minValue;

            int[] arr = new int[n];
            Random ran = new Random((int)DateTime.Now.Ticks);

            bool flag = true;
            for (int i = 0; i < n; i++)
            {
                do
                {
                    int val = ran.Next(minValue, maxValue);
                    if (!IsDuplicates(ref arr, val))
                    {
                        arr[i] = val;
                        flag = false;
                    }
                } while (flag);
                if (!flag)
                    flag = true;
            }
            return arr;
        }

        // 查检当前生成的随机数是否重复
        public bool IsDuplicates(ref int[] arr, int currRandNum)
        {
            bool flag = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == currRandNum)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }
}

using System;
using System.Collections;
namespace ChiyoS.Draw.Komari
{
    class RandomF2
    {


        // n 为生成随机数个数
        public int[] GenerateUniqueRandom(int minValue, int maxValue, int n)
        {
            //如果生成随机数个数大于指定范围的数字总数，则最多只生成该范围内数字总数个随机数
            if (n > maxValue - minValue + 1)
                n = maxValue - minValue + 1;

            int maxIndex = maxValue - minValue + 2;// 索引数组上限
            int[] indexArr = new int[maxIndex];
            for (int i = 0; i < maxIndex; i++)
            {
                indexArr[i] = minValue - 1;
                minValue++;
            }

            Random ran = new Random();
            int[] randNum = new int[n];
            int index;
            for (int j = 0; j < n; j++)
            {
                index = ran.Next(1, maxIndex - 1);// 生成一个随机数作为索引

                //根据索引从索引数组中取一个数保存到随机数数组
                randNum[j] = indexArr[index];

                // 用索引数组中最后一个数取代已被选作随机数的数
                indexArr[index] = indexArr[maxIndex - 1];
                maxIndex--; //索引上限减 1
            }
            return randNum;
        }
    }
}

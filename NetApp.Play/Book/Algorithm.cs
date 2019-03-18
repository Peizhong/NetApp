using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Play.Book
{
    public class Algorithm
    {
        public void Sort(int type)
        {

        }

        public void Search(int type)
        {

        }

        public void BubbleSort(int[] array)
        {
            int len = array.Length;
            bool flag = true;
            while (flag)
            {
                flag = false;
                for (int i = 0; i < len - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                        flag = true;
                    }
                }
                len--;
            }
        }

        public void QuickSort(int[] array)
        {

        }

        public void InsertSort(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                {
                    int temp = array[i];
                    int j;
                    for (j = i - 1; j >= 0 && temp < array[j]; j--)
                    {
                        array[j + 1] = array[j];
                    }
                    array[j + 1] = temp;
                }

            }
        }

        public void ShellSort(int[] array)
        {
            int n = array.Length;
            int h;
            for (h = n / 2; h > 0; h /= 2)
            {
                for (int i = h; i < n; i++)
                {
                    for (int j = i - h; j >= 0; j -= h)
                    {
                        if (array[j] > array[j + h])
                        {
                            int temp = array[j];
                            array[j] = array[j + h];
                            array[j + h] = temp;
                        }
                    }
                }
            }
        }
    }
}

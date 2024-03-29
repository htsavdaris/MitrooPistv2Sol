﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MitrooPistv2.API
{
    public static class clsExtensions
    {

        public static void Shuffle<T>(this IList<T> list)
        {
            if (list != null)
            {
                Random rng = new Random();
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }

    }
}

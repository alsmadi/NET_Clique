using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GraphMaxClique
{
    public class MultipleC
    {
        public int[] degree;   // degree of vertices
        public int[,] A;      // 0/1 adjacency matrix
        public int n;          // n vertices
        public long nodes;     // number of decisions
        long timeLimit; // milliseconds
        long cpuTime;   // milliseconds
        public int maxSize;    // size of max clique
        int style;      // used to flavor algorithm
        public int[] solution; // as it says

        public MultipleC(int n, int[,] A, int[] degree)
        {
            this.n = n;
            this.A = A;
            this.degree = degree;
            nodes = maxSize = 0;
            //cpuTime = timeLimit = -1;
            style = 1;
            solution = new int[n+1];
        }

        public void search()
        {
            DateTime start;
            TimeSpan time;

            start = DateTime.Now;

            //Do something here

            time = DateTime.Now - start;
           
            nodes = 0;
            // ArrayList<Integer> C = new ArrayList<Integer>();
            ArrayList C = new ArrayList();
            ArrayList P = new ArrayList();
            //List<int>[] C = new List<int>[4];
            //List<int>[] P = new List<int>[n];
            //ArrayList<Integer> P = new ArrayList<Integer>(n);
            for (int i = 0; i <= n; i++) { P.Add(i); }
            expand(C, P);
        }

        void expand(ArrayList C, ArrayList P)
        {
         //   if (timeLimit > 0 && System.currentTimeMillis() - cpuTime >= timeLimit) return;
            nodes++;
            for (int i = P.Count-1 ; i >= 0; i--)
            {
                if (C.Count + P.Count <= maxSize) return;
                int v = int.Parse(P[i].ToString());
                C.Add(v);
                ArrayList newP = new ArrayList();
                foreach (int w in P)
                {
                    if (A[v, w] == 1)
                    {
                        newP.Add(w);
                    }
                }
                if (newP.Count==0 && C.Count > maxSize) saveSolution(C);
                if (newP.Count != 0) expand(C, newP);
                C.Remove(v);
                P.Remove(v);

                //C.remove((Integer)v);
                //P.remove((Integer)v);
            }
        }

    /*    public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        } */
        void saveSolution(ArrayList C)
        {
            //   Fill(solution, 0);
            ArrayExtensions.Fill(solution, 0);
            foreach (int i in C) solution[i] = 1;
            maxSize = C.Count;
        }

        
    }

    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }
    }
}

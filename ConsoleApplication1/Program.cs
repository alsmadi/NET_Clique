using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.IO;
using System.Collections; // BitArray


namespace GraphMaxClique
{
  class GraphStructureProgram
  {
        static int[] degree;   // degree of vertices
        static int[,] A;      // 0/1 adjacency matrix
        static int n;          // n vertices
        static long nodes;

        static void readDIMACS(String fname)
        {
            String s = "";
            
            StreamReader sr = new StreamReader(fname);
            String line = null;
            try {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("p") == true && line.IndexOf('p')==0)
                    {
                        string[] temp = line.Split(' ');
                        n = int.Parse(temp[2]);
                        int m = int.Parse(temp[3]);
                        degree = new int[n+1];
                        A = new int[n+1, n+1];
                    }

                    //System.out.println("NO of edges.."+m);


                    else if (line.Contains("e") == true && line.IndexOf('e') == 0)
                    {
                        string[] temp = line.Split(' ');
                        //while (sc.hasNext()){
                        // s     = sc.next(); // skip "edge"
                        int i = int.Parse(temp[1]);
                        int j = int.Parse(temp[2]);
                        degree[i]++; degree[j]++;
                        //    System.out.println("degree of.."+ i+"=="+ degree[i]+
                        //          "degree of.."+ i+"=="+ degree[j]);
                        A[i, j] = A[j, i] = 1;
                    }

                }
            }
            catch (Exception ex) { 
            }
            sr.Close();
        }
        public static string graphFile = null;
        public static void Main(string[] args)
    {
           
            try
      {
        Console.WriteLine("\nBegin graph for maximum clique demo\n");
        
        //string graphFile = "..\\..\\SimpleGraph.txt";
        //MyGraph.ValidateGraphFile(graphFile, "SIMPLE");

       // graphFile = "..\\..\\DimacsGraph.clq";
        string graphFile = "..\\..\\Test.clq";
        //String fname = "DIMACS_cliques\\Test.clq";
          //      System.out.println("read data");
                readDIMACS(graphFile);
                Console.WriteLine("Validating DIMACS format graph file " + graphFile);
        MyGraph.ValidateGraphFile(graphFile, "DIMACS");
        Console.WriteLine("Graph file validation complete");

        //MyGraph graph = new MyGraph(graphFile, "SIMPLE");
        Console.WriteLine("\nLoading graph into memory");
        MyGraph graph = new MyGraph(graphFile, "DIMACS");
              //  n = graph.NumberNodes;
             //   degree = graph.NumberEdges;
        MultipleC mc = new MultipleC(n, A, degree);
                mc.search();

                Console.WriteLine("Max Clique");
                Console.WriteLine(mc.maxSize + " " + mc.nodes + " " );
                for (int i = 0; i < mc.n; i++) if (mc.solution[i] == 1) {
                        Console.WriteLine(i + " ");
                    }
                Console.WriteLine("\nValidating graph data structure");
        graph.ValidateGraph();
        Console.WriteLine("Graph data structure validation complete");
                string name = GraphStructureProgram.graphFile + "graph.csv";
         StreamWriter sw = new StreamWriter(name);
         sw.WriteLine("\nGraph adjaceny representation: \n");
                Console.WriteLine("\nGraph adjaceny representation: \n");
               
       //         Console.WriteLine(graph.ToString());
                sw.WriteLine(graph.ToString());
                sw.Close();
        //graph.ValidateGraph();

        Console.WriteLine("\nAre nodes 5 and 8 adjacent? " + graph.AreAdjacent(5,8));
                for (int i = 0; i < graph.NumberNodes; i++)
                {
                //    Console.WriteLine("Number neighbors of node"+  i +"= " + graph.NumberNeighbors(i));
                }
                //Console.WriteLine(graph.NumberEdges);
                //Console.WriteLine(graph.NumberNeighbors(4));
                //Console.WriteLine(graph.AreAdjacent(2, 4));
                //Console.WriteLine(graph.AreAdjacent(4, 2));
                //Console.WriteLine(graph.AreAdjacent(2, 6));

                graph.writeFriends();

        Console.WriteLine("\nEnd graph for maximum clique demo\n");
        Console.ReadLine();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Fatal: " + ex.Message);
        Console.ReadLine();
      }
    } // Main
  } // class Program

  public class MyGraph
  {
    private BitMatrix data;
    private int numberNodes;
    private int numberEdges;
    private int[] numberNeighbors; // node degrees
        private int[][] NodesNeighbors;
        static int[] degree;   // degree of vertices
        static int[][] A;      // 0/1 adjacency matrix
        static int n;
        public MyGraph(string graphFile, string fileFormat)
    {
      if (fileFormat.ToUpper() == "DIMACS")
        LoadDimacsFormatGraph(graphFile);
      else
        throw new Exception("Format " + fileFormat + " not supported");
    }

        //private void LoadSimpleFormatGraph(string graphFile)
        //{
        //  FileStream ifs = new FileStream(graphFile, FileMode.Open);
        //  StreamReader sr = new StreamReader(ifs);
        //  string line = "";
        //  string[] tokens = null;
        //  string[] subtokens = null;
        //  int numNodes = 0;
        //  int numEdges = 0;
        //  while ((line = sr.ReadLine()) != null)
        //  {
        //    line = line.Trim();
        //    if (line.StartsWith("//") == false)
        //      ++numNodes;
        //  }
        //  sr.Close();
        //  ifs.Close();

        //  this.data = new BitMatrix(numNodes);
        //  this.numberNeighbors = new int[numNodes];

        //  ifs = new FileStream(graphFile, FileMode.Open);
        //  sr = new StreamReader(ifs);

        //  while ((line = sr.ReadLine()) != null)
        //  {
        //    line = line.Trim();
        //    if (line.StartsWith("//") == true) continue;
        //    tokens = line.Split(':');
        //    int fromNode = int.Parse(tokens[0]);
        //    subtokens = tokens[1].Split(',');
        //    numEdges += subtokens.Length;
        //    this.numberNeighbors[fromNode] = subtokens.Length;
        //    for (int j = 0; j < subtokens.Length; ++j)
        //    {

        //      int toNode = int.Parse(subtokens[j]);
        //      this.data.SetValue(fromNode, toNode, true);
        //      this.data.SetValue(toNode, fromNode, true);
        //    }
        //  }
        //  sr.Close();
        //  ifs.Close();

        //  this.numberNodes = numNodes;
        //  this.numberEdges = numEdges / 2;
        //  return;
        //} // LoadSimpleFormatGraph

       /* static void readDIMACS(string graphFile) 
        {
            FileStream ifs = new FileStream(graphFile, FileMode.Open);
            StreamReader sr = new StreamReader(ifs);
            string line = "";
            string[] tokens = null;

            // advance to and get the p line (ex: "p edge 9 16")
            line = sr.ReadLine(); // read first line of file as a priming read
            line = line.Trim();
            while (line != null && line.StartsWith("p") == false)
            {
                line = sr.ReadLine();
                line = line.Trim();
            }

            tokens = line.Split(' ');
            while (sc.hasNext() && !s.equals("p")) s = sc.next();
            sc.next();
            n      = sc.nextInt();
    int m = sc.nextInt();
        degree = new int[n];
	A      = new int[n][n];
	while (sc.hasNext()){
	    s     = sc.next(); // skip "edge"
	    int i = sc.nextInt() - 1;
        int j = sc.nextInt() - 1;
        degree[i]++; degree[j]++;
	    A[i][j] = A[j][i] = 1;
	}
    sc.close();
    } */
private void LoadDimacsFormatGraph(string graphFile)
    {
      FileStream ifs = new FileStream(graphFile, FileMode.Open);
      StreamReader sr = new StreamReader(ifs);
      string line = "";
      string[] tokens = null;

      // advance to and get the p line (ex: "p edge 9 16")
      line = sr.ReadLine(); // read first line of file as a priming read
      line = line.Trim();
      while (line != null && line.StartsWith("p") == false)
      {
        line = sr.ReadLine();
        line = line.Trim();
      }

      tokens = line.Split(' ');
      int numNodes = int.Parse(tokens[2]); // number nodes
      int numEdges = int.Parse(tokens[3]); // number edges
 
      sr.Close(); ifs.Close();

      this.data = new BitMatrix(numNodes+1);

      ifs = new FileStream(graphFile, FileMode.Open); // reopen file
      sr = new StreamReader(ifs);
      while ((line = sr.ReadLine()) != null)
      {
        line = line.Trim();
        if (line.StartsWith("e") == true) // (ex: "e 7 4")
        {
          tokens = line.Split(' ');
          int nodeA = int.Parse(tokens[1]) ; // DIMACS is 1-based. subtract 1 to convert to 0-based
          int nodeB = int.Parse(tokens[2]) ;
          data.SetValue(nodeA, nodeB, true);
          data.SetValue(nodeB, nodeA, true);
        }
      }
      sr.Close(); ifs.Close();
      
      this.numberNeighbors = new int[numNodes+1];
      for (int row = 1; row <= numNodes; ++row)
      {
        int count = 0;
        for (int col = 1; col <= numNodes; ++col)
        {
          if (data.GetValue(row, col) == true)
            ++count;
        }
        numberNeighbors[row] = count;
                //NodesNeighbors
      }
      
      this.numberNodes = numNodes;
      this.numberEdges = numEdges;
      return;
    }

    public int NumberNodes
    {
      get { return this.numberNodes; }
    }

    public int NumberEdges
    {
      get { return this.numberEdges; }
    }

        public void writeFriends()
        {
            string name = GraphStructureProgram.graphFile + "Friends.csv";
            StreamWriter sw = new StreamWriter(name);
            sw.WriteLine("Node Number"+","+"Number of Freinds");
            int col=1;
            try {
                for (col = 1; col <= this.NumberNodes; col++)
                {
                   // int temp = col + 1;
                    //  for (int i = this.numberNodes; i < this.NumberNeighbors.GetLength(1); i++) { 
                    sw.WriteLine(col + "," + this.NumberNeighbors(col));
                }
            }
            catch(Exception ex) {
                System.Console.WriteLine("Crash in.."+ col);
            }

            sw.Close();
        }
    public int NumberNeighbors(int node)
    {
      return this.numberNeighbors[node];
    }

    public bool AreAdjacent(int nodeA, int nodeB)
    {
      if (this.data.GetValue(nodeA, nodeB) == true)
        return true;
      else
        return false;
    }

    public override string ToString()
    {
      string s = "";
      for (int i = 0; i < this.data.Dim; ++i)
      {
        s += i + ": ";
        for (int j = 0; j < this.data.Dim; ++j)
        {
          if (this.data.GetValue(i, j) == true)
            s += j + " ";
        }
        s += Environment.NewLine;
      }
      return s;
    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------

    public static void ValidateGraphFile(string graphFile, string fileFormat)
    {
      if (fileFormat.ToUpper() == "DIMACS")
        ValidateDimacsGraphFile(graphFile);
      else
        throw new Exception("Format " + fileFormat + " not supported");
    }

    //public static void ValidateSimpleGraphFile(string graphFile)
    //{
    //  FileStream ifs = new FileStream(graphFile, FileMode.Open);
    //  StreamReader sr = new StreamReader(ifs);
    //  string line = "";
    //  string[] tokens = null;
    //  string[] subtokens = null;

    //  while ((line = sr.ReadLine()) != null)
    //  {
    //    line = line.Trim();
    //    if (line.StartsWith("//") == true)
    //      continue;
    //    try
    //    {
    //      tokens = line.Split(':');
    //      int fromNode = int.Parse(tokens[0]);
    //      subtokens = tokens[1].Split(',');
    //    }
    //    catch
    //    {
    //      throw new Exception("Error parsing line = " + line + " in ValidateSimpleGraphFile");
    //    }
    //  } // while

    //  sr.Close();
    //  ifs.Close();
    //  return;
    //}

    public static void ValidateDimacsGraphFile(string graphFile)
    {
      FileStream ifs = new FileStream(graphFile, FileMode.Open);
      StreamReader sr = new StreamReader(ifs);
      string line = "";
      string[] tokens = null;
 
      while ((line = sr.ReadLine()) != null)
      {
        line = line.Trim();
        if (line.StartsWith("c") == false && line.StartsWith("p") == false &&
          line.StartsWith("e") == false)
          throw new Exception("Unknown line type: " + line + " in ValidateDimacsGraphFile");

        try
        {
          if (line.StartsWith("p"))
          {
            tokens = line.Split(' ');
            int numNodes = int.Parse(tokens[2]);
            int numEdges = int.Parse(tokens[3]);
          }
          else if (line.StartsWith("e"))
          {
            tokens = line.Split(' ');
            int nodeA = int.Parse(tokens[1]);
            int nodeB = int.Parse(tokens[2]);
          }
        }
        catch
        {
          throw new Exception("Error parsing line = " + line + " in ValidateDimacsGraphFile");
        }
      }

      sr.Close();
      ifs.Close();
      return;
    } // ValidateDimacsGraphFile

    public void ValidateGraph()
    {
      // total number edges items must be an even number
      int numConnections = 0;
      for (int i = 0; i < this.data.Dim; ++i)
      {
        for (int j = 0; j < this.data.Dim; ++j)
        {
          if (this.data.GetValue(i, j) == true)
            ++numConnections;
        }
      }
      if (numConnections % 2 != 0)
        throw new Exception("Total number of connections in graph is " + numConnections + ". Should be even");

      // fully symmetric
      for (int i = 0; i < this.data.Dim; ++i)
      {
        if (this.data.GetValue(i, i) == true)
          throw new Exception("Node " + i + " is connected to itself");
        for (int j = 0; j < this.data.Dim; ++j)
        {
          if (this.data.GetValue(i, j) != this.data.GetValue(j, i))
            throw new Exception("Graph is not symmetric at " + i + " and " + j);
        }
      }

      return;
    } // ValidateGraph

    

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------

    private class BitMatrix
    {
      private BitArray[] data; // an array of arrays
      public readonly int Dim; // dimension

      public BitMatrix(int n)
      {
        this.data = new BitArray[n];
        for (int i = 0; i < data.Length; ++i)
        {
          this.data[i] = new BitArray(n);
        }
        this.Dim = n;
      }
      public bool GetValue(int row, int col)
      {
        return data[row][col];
      }
      public void SetValue(int row, int col, bool value)
      {
        data[row][col] = value;
      }
      public override string ToString()
      {
        string s = "";
        for (int i = 0; i < data.Length; ++i)
        {
          for (int j = 0; j < data[i].Length; ++j)
          {
            if (data[i][j] == true) s += "1 "; else s += "0 ";
          }
          s += Environment.NewLine;
        }
        return s;
      }


    } // class BitMatrix


  } // class MyGraph

  // ==================================================================================================================================================================

  //private class BitMatrix
  //{
  //  private BitArray[] data; // an array of arrays
  //  public readonly int Dim; // dimension

  //  public BitMatrix(int n)
  //  {
  //    this.data = new BitArray[n];
  //    for (int i = 0; i < data.Length; ++i)
  //    {
  //      this.data[i] = new BitArray(n);
  //    }
  //    this.Dim = n;
  //  }
  //  public bool GetValue(int row, int col)
  //  {
  //    return data[row][col];
  //  }
  //  public void SetValue(int row, int col, bool value)
  //  {
  //    data[row][col] = value;
  //  }
  //  public override string ToString()
  //  {
  //    string s = "";
  //    for (int i = 0; i < data.Length; ++i)
  //    {
  //      for (int j = 0; j < data[i].Length; ++j)
  //      {
  //        if (data[i][j] == true) s += "1 "; else s += "0 ";
  //      }
  //      s += Environment.NewLine;
  //    }
  //    return s;
  //  }
    

  //} // class BitMatrix

  // ==================================================================================================================================================================



} // ns

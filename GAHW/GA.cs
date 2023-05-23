using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAHW
{
    public class CompareByFitness : IComparer<Citizen>
    {
        // Implement the IComparable interface. 
        public int Compare(Citizen obj1, Citizen obj2)
        {
            return obj1.Fitness.CompareTo(obj2.Fitness);
        }
    } 

    public class Citizen
    {
        public char[] Value;
        public int Fitness;

        public Citizen(int size)
        {
            Value = new char[size];
            Fitness = 0;
        }

        public void RandomValue(Random r)
        {
            for (int j = 0; j < Value.Length; j++)
            {
                Value[j] = (char)((126 * r.NextDouble()) + 32);
            }
        }

        public void CalculateFitness(char[] target)
        {
            int fitness = 0;
            for (int j = 0; j < Value.Length; j++)
            {
                fitness += Math.Abs(Value[j] - target[j]);
            }
            Fitness = fitness;
        }

        public override string ToString()
        {
            return string.Format("f:{1} v:{0}", new String(Value), Fitness);
        }
    }

    public class GASearchResult
    {
        public Citizen Best;
        public int GenerationsRun;
    }

    public delegate void SearchResultHandler(GASearchResult result);

    public class GA
    {
        public event SearchResultHandler BestOfGenerationFound;
        public event SearchResultHandler Finished;
        public List<Citizen> Population;
        public List<Citizen> NextGeneration;
        public float elitismRate = 0.10f;
        public float mutationRate = 0.25f;
        public string targetWord;
        public int maxGenerations;
        public int maxPopulationPerGeneration;

        public void StartGeneration()
        { 
            //Final string to find
            char[] target = targetWord.ToCharArray();
            //Size of string to easy search process
            int targetSize = target.Length;
            Population = new List<Citizen>(maxPopulationPerGeneration);
            NextGeneration = new List<Citizen>(maxPopulationPerGeneration);

            //Generate Initial population
            GenerateInitialPopulation(targetSize, maxPopulationPerGeneration);
            Citizen best = null;
            int gen = 0;
            for (; gen < maxGenerations; gen++)
            {
                //Start processing generation

                //Fitness calculation
                CalculateFitness(target);
                //Print best so far
                best = GetBest();

                //Return best match so far
                if (BestOfGenerationFound != null)
                {
                    GASearchResult result = new GASearchResult();
                    result.Best = best;
                    result.GenerationsRun = gen;
                    BestOfGenerationFound(result);
                }

                //Get best & check
                //If equal, done (optimal)
                if (best.Fitness == 0) break;

                //Mate
                MatePopulation(targetSize, maxPopulationPerGeneration, elitismRate, mutationRate);

                //Swap
                SwapPopulation();
            }

            //Return best match
            if (Finished != null)
            {
                GASearchResult result = new GASearchResult();
                result.Best = best;
                result.GenerationsRun = gen;
                Finished(result);
            }
        }

        private void GenerateInitialPopulation(int targetSize, int maxPopulationPerGeneration)
        {
            Random r = new Random();
            for (int i = 0; i < maxPopulationPerGeneration; i++)
            {
                Citizen c = new Citizen(targetSize);
                c.RandomValue(r);
                Population.Add(c);
            }
        }

        private void CalculateFitness(char[] target)
        {
            foreach (Citizen c in Population)
            {
                c.CalculateFitness(target);
            }
        }

        private Citizen GetBest()
        {
            Citizen best = null;
            foreach (Citizen c in Population)
            {
                if (best == null)
                {
                    best = c;
                    continue;
                }
                if (c.Fitness < best.Fitness) best = c;
            }
            return best;
        }

        private void MatePopulation(int targetSize, int maxPopulationPerGeneration, float elitismRate, float mutationRate)
        {
            //Elitism
            //Mutation
            //Crossover
            int elitSize = (int)(maxPopulationPerGeneration * elitismRate);
            Random r = new Random();

            Elitism(elitSize);

            // Mate the rest
            for (int i = elitSize; i < maxPopulationPerGeneration; i++)
            {
                NextGeneration.Add(new Citizen(targetSize));

                int i1 = (int)(r.NextDouble() * maxPopulationPerGeneration);
                int i2 = (int)(r.NextDouble() * maxPopulationPerGeneration);
                int spos = (int)(r.NextDouble() * targetSize);

                NextGeneration[i].Value =
                    (new String(Population[i1].Value).Substring(0, spos) +
                    new String(Population[i2].Value).Substring(spos, targetSize - spos)).ToCharArray();

                if (r.NextDouble() < mutationRate) Mutate(NextGeneration[i], targetSize, r);
            }
        }

        private void Mutate(Citizen citizen, int targetSize, Random r)
        {
            int ipos = (int)(r.NextDouble() * targetSize);
            int mutantGene = (int)(r.NextDouble() * 126) + 32;

            citizen.Value[ipos] = (char)mutantGene;
        }

        private void Elitism(int elitSize)
        {
            Population.Sort(new CompareByFitness());
            for (int i = 0; i < elitSize; i++)
            {
                NextGeneration.Add(Population[i]);
            }
        }

        private void SwapPopulation()
        {
            Population.Clear();
            foreach (Citizen c in NextGeneration)
            {
                Population.Add(c);
            }
            NextGeneration.Clear();
        }
    }
}

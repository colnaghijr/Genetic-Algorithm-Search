using System;
using System.Collections.Generic;

/*
    MIT License

    Copyright (c) 2008 Roberto Colnaghi Jr

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

/**
 * 
 * This is a VERY simplified version of a genetic algorithm.
 * 
 * It is meant to be used as a learning tool, to turn basic terms and concepts into C# code.
 * 
 * - Population: It is a subset of all the possible solutions to a given problem.
 * - Citizen(aka Individual): an individual in a population that holds genetic data (Chromosome, Fitness)
 * - Chromosome: A chromosome is one such solution to the given problem.
 * - Gene: A gene is one element position of a chromosome.
 * - Crossover: For each new solution to be produced, a pair of "parent" solutions is selected for breeding from the pool selected previously.
 * - Mutation: Random changes in the genes of the offspring.
 * - Elitism: A practical variant of the general process of constructing a new population is to allow the best 
 *            organism(s) from the current generation to carry over to the next, unaltered. This strategy is known as 
 *            elitist selection and guarantees that the solution quality obtained by the GA will not 
 *            decrease from one generation to the next.
 * 
 * The Genetic Algorithm (GA) is a search heuristic that mimics the process of natural evolution.
 * 
 * It follows the basic steps:
 * 1. Initial population
 * 2. Fitness function
 * 3. Selection
 * 4. Crossover
 * 5. Mutation
 */

namespace GeneticAlgorithm {
    public class CompareByFitness : IComparer<Citizen> {
        // Implement the IComparable interface. 
        public int Compare(Citizen obj1, Citizen obj2) {
            return obj1.Fitness.CompareTo(obj2.Fitness);
        }
    }

    // Entity of a given population (data model)
    public class Citizen {
        // Genes structure
        public char[] Chromosome;
        // How close to the target is this citizen
        public int Fitness;
        // Simple random generator
        private static Random random = new Random();

        public Citizen(int size) {
            Chromosome = new char[size];
            Fitness = 0;

            RandomizeGenome();
        }

        // Create an initial random genome for the citizen
        protected void RandomizeGenome() {
            for (int j = 0; j < Chromosome.Length; j++) {
                Chromosome[j] = (char)((126 * random.NextDouble()) + 32);
            }
        }

        // Calculate fitness based on a given target
        public void CalculateFitness(char[] target) {
            int fitness = 0;
            for (int j = 0; j < Chromosome.Length; j++) {
                fitness += Math.Abs(Chromosome[j] - target[j]);
            }
            Fitness = fitness;
        }

        public override string ToString() {
            return string.Format("f:{1} c:[{0}]", new String(Chromosome), Fitness);
        }
    }

    // Holds result of a genetic algorithm search
    public class GeneticAlgorithmSearchResult {
        public Citizen Best;
        public int GenerationsRun;
    }

    public delegate void SearchResultHandler(GeneticAlgorithmSearchResult result);

    public class GeneticaAlgorithmSearch {
        public event SearchResultHandler BestOfGenerationFound;
        public event SearchResultHandler Finished;
        // holds current population of citizens
        public List<Citizen> Population;
        // next generation of citizens (iteration +1)
        public List<Citizen> NextGeneration;
        // How much of the population to keep for the next generation
        public float elitismRate = 0.10f;
        // How much random mutation to apply to the offspring
        public float mutationRate = 0.25f;
        // target word to find (AKA target genome)
        public string targetWord;
        // limits search to a given number of generations
        public int maxGenerations;
        // limits the number of citizens per generation
        public int maxPopulationPerGeneration;
        // Simple random generator
        private static Random random = new Random();

        public void StartSearch() {
            //Final Chromosome to find
            char[] target = targetWord.ToCharArray();
            //Chromosome Size
            int targetSize = target.Length;
            Population = new List<Citizen>(maxPopulationPerGeneration);
            NextGeneration = new List<Citizen>(maxPopulationPerGeneration);

            //Generate Initial population for the search
            GenerateInitialPopulation(targetSize, maxPopulationPerGeneration);
            //holds best citizen so far
            Citizen best = null;

            int gen = 0;
            for (; gen < maxGenerations; gen++) {
                //Start processing generation

                //Fitness calculation
                CalculateFitness(target);
                //Print best so far
                best = GetBest();

                //Return best match so far
                BestOfGenerationFound?.Invoke(new GeneticAlgorithmSearchResult { Best = best, GenerationsRun = gen });

                //Get best & check
                //If equal, done (optimal find)
                if (best.Fitness == 0) break;

                //Mate citizens into next generation
                MatePopulation(targetSize, maxPopulationPerGeneration, elitismRate, mutationRate);

                //Swap
                SwapPopulation();
            }

            //Return best match
            Finished?.Invoke(new GeneticAlgorithmSearchResult { Best = best, GenerationsRun = gen });
        }

        //Create initial population for this search
        private void GenerateInitialPopulation(int targetSize, int maxPopulationPerGeneration) {
            for (int i = 0; i < maxPopulationPerGeneration; i++) {
                Population.Add(new Citizen(targetSize));
            }
        }

        // Calculate fitness of each population member
        private void CalculateFitness(char[] target) {
            foreach (Citizen c in Population) {
                c.CalculateFitness(target);
            }
        }

        // Finds the best citizen in the current population
        private Citizen GetBest() {
            Citizen best = null;
            foreach (Citizen c in Population) {
                if (best == null || c.Fitness < best.Fitness)
                {
                    best = c;
                }
            }
            return best;
        }

        // Mate the population applying Crossover, Mutation and Elitism
        private void MatePopulation(int targetSize, int maxPopulationPerGeneration, float elitismRate, float mutationRate) {
            int elitSize = (int)(maxPopulationPerGeneration * elitismRate);

            // Keep the fittest
            Elitism(elitSize);

            // Mate the rest
            for (int i = elitSize; i < maxPopulationPerGeneration; i++) {
                int i1 = random.Next(maxPopulationPerGeneration);
                int i2 = random.Next(maxPopulationPerGeneration);
                int spos = random.Next(targetSize);

                char[] newChromosome = new char[targetSize];
                Array.Copy(Population[i1].Chromosome, 0, newChromosome, 0, spos);
                Array.Copy(Population[i2].Chromosome, spos, newChromosome, spos, targetSize - spos);

                Citizen offspring = new Citizen(targetSize) { Chromosome = newChromosome };

                if (random.NextDouble() < mutationRate) Mutate(offspring, targetSize);

                NextGeneration.Add(offspring);
            }
        }

        // Mutate a citizen
        private void Mutate(Citizen citizen, int targetSize) {
            int ipos = random.Next(targetSize);
            citizen.Chromosome[ipos] = (char)random.Next(32, 127);
        }

        // Keep the fitest citizens
        private void Elitism(int elitSize) {
            Population.Sort(new CompareByFitness());
            for (int i = 0; i < elitSize; i++) {
                NextGeneration.Add(Population[i]);
            }
        }

        // Swap current population with next generation
        private void SwapPopulation() {
            var temp = Population;
            Population = NextGeneration;
            NextGeneration = temp;
            NextGeneration.Clear();
        }
    }
}

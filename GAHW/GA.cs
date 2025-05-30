using System;
using System.Collections.Generic;
using System.Threading;

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
    // Entity of a given population (data model)
    public class Citizen {
        // Genes structure
        public char[] Chromosome { get; internal set; } // Changed to internal set for assignment in MatePopulation
        // How close to the target is this citizen
        public int Fitness { get; set; } // Fitness can be set by CalculateFitness

        private readonly Random _randomInstance;
        private const int PrintableCharMin = 32;
        private const int PrintableCharMax = 126;

        public Citizen(int size, Random randomInstance) {
            Chromosome = new char[size];
            Fitness = 0;
            _randomInstance = randomInstance ?? throw new ArgumentNullException(nameof(randomInstance)); // Ensure randomInstance is not null

            RandomizeGenome();
        }

        // Create an initial random genome for the citizen
        protected void RandomizeGenome() {
            for (int j = 0; j < Chromosome.Length; j++) {
                // Generate a random printable ASCII character
                Chromosome[j] = (char)(_randomInstance.Next(PrintableCharMin, PrintableCharMax + 1));
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
            return $"f:{Fitness} c:[{new string(Chromosome)}]";
        }
    }

    // Holds result of a genetic algorithm search
    public class GeneticAlgorithmSearchResult {
        public Citizen Best;
        public int GenerationsRun;
    }

    public delegate void SearchResultHandler(GeneticAlgorithmSearchResult result);

    public class GeneticAlgorithmSearch {
        // Nested class for comparing citizens by fitness
        private class CompareByFitness : IComparer<Citizen> {
            public int Compare(Citizen obj1, Citizen obj2) {
                return obj1.Fitness.CompareTo(obj2.Fitness);
            }
        }

        public event SearchResultHandler BestOfGenerationFound;
        public event SearchResultHandler Finished;
        
        // holds current population of citizens
        public List<Citizen> Population { get; private set; } // Encapsulated: public getter, private setter
        public List<Citizen> NextGeneration { get; private set; } // Encapsulated: public getter, private setter

        // How much of the population to keep for the next generation
        public float ElitismRate { get; internal set; } = 0.10f;
        // How much random mutation to apply to the offspring
        public float MutationRate { get; internal set; } = 0.25f;
        // target word to find (AKA target genome)
        public string TargetWord { get; internal set; }
        // limits search to a given number of generations
        public int MaxGenerations { get; internal set; }
        // limits the number of citizens per generation
        public int MaxPopulationPerGeneration { get; internal set; }
        
        // Simple random generator - kept static as it's used across all instances of the search,
        // but could be instance-based if multiple searches needed different seedings.
        private static readonly Random random = new Random(); // Made readonly

        private const int MinGeneValue = 32; // Printable ASCII
        private const int MaxGeneValue = 126; // Printable ASCII (inclusive, hence +1 in Next)

        public void StartSearch(CancellationToken cancellationToken)
        {
            char[] target = TargetWord.ToCharArray();
            int targetSize = target.Length;

            InitializeSearch(targetSize, MaxPopulationPerGeneration);
            
            RunGenerationLoop(cancellationToken, target, targetSize);
        }

        private void InitializeSearch(int targetSize, int maxPopulation)
        {
            Population = new List<Citizen>(maxPopulation);
            NextGeneration = new List<Citizen>(maxPopulation);
            GenerateInitialPopulation(targetSize, maxPopulation);
        }

        private void RunGenerationLoop(CancellationToken cancellationToken, char[] target, int targetSize)
        {
            Citizen best = null;
            int currentGeneration = 0;
            for (; currentGeneration < MaxGenerations; currentGeneration++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Finished?.Invoke(new GeneticAlgorithmSearchResult { Best = best, GenerationsRun = currentGeneration });
                    return;
                }

                ProcessSingleGeneration(target, targetSize, ref best, currentGeneration);

                if (best?.Fitness == 0) // Optimal solution found
                {
                    break;
                }
            }
            Finished?.Invoke(new GeneticAlgorithmSearchResult { Best = best, GenerationsRun = currentGeneration });
        }

        private void ProcessSingleGeneration(char[] target, int targetSize, ref Citizen best, int currentGeneration)
        {
            CalculateFitness(target);
            best = GetBest(); // Update the best citizen found so far

            BestOfGenerationFound?.Invoke(new GeneticAlgorithmSearchResult { Best = best, GenerationsRun = currentGeneration });

            if (best.Fitness == 0) // If optimal solution is found, no need to mate further
            {
                return;
            }

            MatePopulation(targetSize, MaxPopulationPerGeneration, ElitismRate, MutationRate);
            SwapPopulation();
        }

        //Create initial population for this search
        private void GenerateInitialPopulation(int targetSize, int maxPopulationPerGeneration) {
            for (int i = 0; i < maxPopulationPerGeneration; i++) {
                Population.Add(new Citizen(targetSize, random));
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
                int parent1Index = random.Next(Population.Count); // Ensure index is within bounds of current Population
                int parent2Index = random.Next(Population.Count); // Ensure index is within bounds of current Population
                int crossoverPoint = random.Next(targetSize);

                char[] newChromosome = new char[targetSize];
                Array.Copy(Population[parent1Index].Chromosome, 0, newChromosome, 0, crossoverPoint);
                Array.Copy(Population[parent2Index].Chromosome, crossoverPoint, newChromosome, crossoverPoint, targetSize - crossoverPoint);
                
                // Create offspring with the new chromosome, passing the random instance
                Citizen offspring = new Citizen(targetSize, random) { Chromosome = newChromosome };

                if (random.NextDouble() < mutationRate) Mutate(offspring); // Pass only offspring

                NextGeneration.Add(offspring);
            }
        }

        // Mutate a citizen
        private void Mutate(Citizen citizen) { // targetSize is citizen.Chromosome.Length
            int ipos = random.Next(citizen.Chromosome.Length);
            citizen.Chromosome[ipos] = (char)random.Next(MinGeneValue, MaxGeneValue + 1); // Use constants
        }

        // Keep the fitest citizens
        private void Elitism(int elitSize) {
            Population.Sort(new CompareByFitness()); // Uses the nested class
            for (int i = 0; i < elitSize && i < Population.Count; i++) { // Added boundary check for i
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

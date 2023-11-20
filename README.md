# C# Genetic Algorithm Search
This is a VERY simplified version of a genetic algorithm.
 
It is meant to be used as a learning tool, to turn basic terms and concepts into C# code.

- Population: It is a subset of all the possible solutions to a given problem.
- Citizen(aka Individual): an individual in a population that holds genetic data (Chromosome, Fitness)
- Chromosome: A chromosome is one such solution to the given problem.
- Gene: A gene is one element position of a chromosome.
- Crossover: For each new solution to be produced, a pair of "parent" solutions is selected for breeding from the pool selected previously.
- Mutation: Random changes in the genes of the offspring.
- Elitism: A practical variant of the general process of constructing a new population is to allow the best 
           organism(s) from the current generation to carry over to the next, unaltered. This strategy is known as 
           elitist selection and guarantees that the solution quality obtained by the GA will not 
           decrease from one generation to the next.

The Genetic Algorithm (GA) is a search heuristic that mimics the process of natural evolution.

It follows the basic steps:
1. Initial population
2. Fitness function
3. Selection
4. Crossover
5. Mutation
using System;
using System.Collections.Generic;
using System.Linq;

public class GeneticAlgorithm<T> 
{
   public List<DNA<T>> OnPopulation { get; private set; }
   public int OnGeneration { get; private set; }
   public float BestFitness { get; private set; }
   public T[] BestGenes { get; private set; }
   
   public float mutationRate;
   public int elitism;
   private List<DNA<T>> newPopulation;
   
   private Random random;
   private float fitnessSum;
   private int dnaSize;
   private Func<T> getRandomGene;
   private Func<int, float> fitnessFunction;

   public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene,
      Func<int, float> fitnessFunction, int elitism, float mutationRate = 0.01f)
   {
      OnGeneration = 1;
      OnPopulation = new List<DNA<T>>(populationSize);
      newPopulation = new List<DNA<T>>(populationSize);
      this.mutationRate = mutationRate;
      this.random = random;
      this.elitism = elitism;
      this.dnaSize = dnaSize;
      this.getRandomGene = getRandomGene;
      this.fitnessFunction = fitnessFunction;

      BestGenes = new T[dnaSize];

      for (int i = 0; i < populationSize; i++)
      {
         OnPopulation.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, populateGenes: true));
      }
   }

   public void NewGeneration(int newDnaNumber = 0, bool canCrossover = false)
   {
      int finalCount = OnPopulation.Count + newDnaNumber;
      if (finalCount <= 0)
      {
         return;
      }

      if (OnPopulation.Count > 0)
      {
         CalculateFitness();
         OnPopulation.Sort(CompareDNA);
      }
      
      newPopulation.Clear();

      for (int i = 0; i < OnPopulation.Count; i++)
      {
         if (i < elitism && i < OnPopulation.Count)
         {
            newPopulation.Add(OnPopulation[i]);
         }
         else if (i < OnPopulation.Count || canCrossover)
         {
            DNA<T> parent1 = ChooseParent();
            DNA<T> parent2 = ChooseParent();

            DNA<T> child = parent1.Crossover(parent2);
            child.Mutate(mutationRate);
            newPopulation.Add(child);
         }
         else
         {
            newPopulation.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, populateGenes: true));
         }
      }
      (OnPopulation, newPopulation) = (newPopulation, OnPopulation);
      OnGeneration++;
   }

   public void CalculateFitness()
   {
      fitnessSum = 0;
      DNA<T> best = OnPopulation[0];
      
      for (int i = 0; i < OnPopulation.Count; i++)
      {
         fitnessSum += OnPopulation[i].CalculateFitness(i);
         if (OnPopulation[i].OnFitness > best.OnFitness)
         {
            best = OnPopulation[i];
         }
      }
      BestFitness = best.OnFitness;
      best.OnGenes.CopyTo(BestGenes, 0);
   }

   private DNA<T> ChooseParent()
   {
      double randomNumber = random.NextDouble() * fitnessSum;
      for (int i = 0; i < OnPopulation.Count; i++)
      {
         if (randomNumber < OnPopulation[i].OnFitness)
         {
            return OnPopulation[i];
         }

         randomNumber -= OnPopulation[i].OnFitness;
      }
      return null;
   }

   public int CompareDNA(DNA<T> firstDna, DNA<T> secondDna)
   {
      if (firstDna.OnFitness > secondDna.OnFitness)
      {
         return -1;
      }
      else if (firstDna.OnFitness < secondDna.OnFitness)
      {
         return 1;
      }
      else
      {
         return 0;
      }
   }
}

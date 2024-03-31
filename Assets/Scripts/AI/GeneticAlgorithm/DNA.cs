using System;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<int> Batch { get; private set; }
    public List<float> Difficulty { get; private set; }
    public List<int> Chromosome { get; private set; }
    
    public int Distance { get; private set; }
    public int Generation { get; private set; }
    public float ScoreEvaluation { get; private set; }

    public DNA(List<int> batch, List<float> difficulty, int distance, int generation = 0)
    {
        Batch = batch;
        Difficulty = difficulty;
        Distance = distance;
        Generation = generation;
        Chromosome = new List<int>();

        System.Random rand = new();
        for (int i = 0; i < batch.Count; i++)
        {
            if (rand.NextDouble() < 0.95)
                Chromosome.Add(0);
            else
                Chromosome.Add(1);
        }
    }
    
    public void CalculateFitness()
    {
        float score = 0;
        
        for (int i = 0; i < Chromosome.Count; i++)
        {
            if (Chromosome[i] == 1)
            {
                score += Difficulty[i];
            }
        }
        if (score > Distance)
            score = 0;
        ScoreEvaluation = score;
    }
    
    public List<DNA> Crossover(DNA otherDna)
    {
        System.Random rand = new ();
        int middleRangeStart = Chromosome.Count / 3;
        int middleRangeEnd = (Chromosome.Count * 2) / 3;
        int cutoff = rand.Next(middleRangeStart + 1, middleRangeEnd + 1);

        List<int> child1 = new List<int>(otherDna.Chromosome.GetRange(0, cutoff));
        child1.AddRange(Chromosome.GetRange(cutoff, Chromosome.Count - cutoff));

        List<int> child2 = new List<int>(this.Chromosome.GetRange(0, cutoff));
        child2.AddRange(otherDna.Chromosome.GetRange(cutoff, otherDna.Chromosome.Count - cutoff));

        List<DNA> children = new List<DNA>
        {
            new DNA(Batch, Difficulty, Distance, Generation + 1),
            new DNA(Batch, Difficulty, Distance, Generation + 1)
        };

        children[0].Chromosome = child1;
        children[1].Chromosome = child2;
        return children;
    }

    public DNA Mutation(double rate)
    {
        System.Random rand = new ();
        List<int> mutatedChromosome = new List<int>(Chromosome);
        
        for (int i = 0; i < Chromosome.Count; i++)
        {
            if (rand.NextDouble() < rate)
            {
                if (Chromosome[i] == 1)
                    Chromosome[i] = 0;
                else
                    Chromosome[i] = 1;
            }
        }
        
        return new DNA(Batch, Difficulty, Distance, Generation)
        {
            Chromosome = mutatedChromosome
        };
    }
}

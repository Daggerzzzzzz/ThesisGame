using System;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    private List<int> batch;
    private List<float> difficulty;
    private List<int> chromosome;
    
    private int distance;
    private int generation;
    private float scoreEvaluation;

    public DNA(List<int> batch, List<float> difficulty, int distance, int generation = 0)
    {
        this.batch = batch;
        this.difficulty = difficulty;
        this.distance = distance;
        this.generation = generation;
        chromosome = new List<int>();

        System.Random rand = new();
        for (int i = 0; i < batch.Count; i++)
        {
            if (rand.NextDouble() < 0.5)
                chromosome.Add(0);
            else
                chromosome.Add(1);
        }
    }
    
    public void CalculateFitness()
    {
        float score = 0;
        
        for (int i = 0; i < chromosome.Count; i++)
        {
            if (chromosome[i] == 1)
            {
                score += difficulty[i];
            }
        }
        if (score > distance)
            score = 0;
        scoreEvaluation = score;
    }
    
    public List<DNA> Crossover(DNA otherDna)
    {
        System.Random rand = new ();
        int middleRangeStart = chromosome.Count / 3;
        int middleRangeEnd = (chromosome.Count * 2) / 3;
        int cutoff = rand.Next(middleRangeStart + 1, middleRangeEnd + 1);
        
        Debug.Log(cutoff);

        List<int> child1 = new List<int>(otherDna.chromosome.GetRange(0, cutoff));
        child1.AddRange(chromosome.GetRange(cutoff, chromosome.Count - cutoff));

        List<int> child2 = new List<int>(this.chromosome.GetRange(0, cutoff));
        child2.AddRange(otherDna.chromosome.GetRange(cutoff, otherDna.chromosome.Count - cutoff));

        List<DNA> children = new List<DNA>
        {
            new DNA(batch, difficulty, distance, generation + 1),
            new DNA(batch, difficulty, distance, generation + 1)
        };

        children[0].chromosome = child1;
        children[1].chromosome = child2;
        return children;
    }

    public DNA Mutation(double rate)
    {
        System.Random rand = new ();
        for (int i = 0; i < chromosome.Count; i++)
        {
            if (rand.NextDouble() < rate)
            {
                if (chromosome[i] == 1)
                    chromosome[i] = 0;
                else
                    chromosome[i] = 1;
            }
        }
        return this;
    }
}

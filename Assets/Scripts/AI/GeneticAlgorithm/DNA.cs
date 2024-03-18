using System;
using System.Collections.Generic;
using UnityEngine;

public class DNA<T>
{
    public T[] OnGenes { get; private set; }
    public float OnFitness { get; private set; }
    
    private readonly System.Random random;
    private readonly Func<T> getRandomGene;
    private readonly Func<int, float> fitnessFunction;
    
    public DNA(int geneLength, System.Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool populateGenes = true)
    {
        OnGenes = new T[geneLength];
        this.random = random;
        this.getRandomGene = getRandomGene;
        this.fitnessFunction = fitnessFunction;

        if (populateGenes)
        {
            for (int i = 0; i < OnGenes.Length; i++)
            {
                OnGenes[i] = getRandomGene();
            }
        }
    }

    public float CalculateFitness(int index)
    {
        OnFitness = fitnessFunction(index);
        return OnFitness;
    }
    
    public DNA<T> Crossover(DNA<T> partner)
    {
        DNA<T> child = new DNA<T>(OnGenes.Length, random, getRandomGene, fitnessFunction, populateGenes: false);
        for (int i = 0; i < OnGenes.Length; i++)
        {
            if (random.NextDouble() < 0.5)
            {
                child.OnGenes[i] = OnGenes[i];
            }
            else
            {
                child.OnGenes[i] = partner.OnGenes[i];
            }
        }
        return child;
    }
    
    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < OnGenes.Length; i++)
        {
            if (random.NextDouble() < mutationRate)
            {
                OnGenes[i] = getRandomGene();
            }
        }
    }
    
}

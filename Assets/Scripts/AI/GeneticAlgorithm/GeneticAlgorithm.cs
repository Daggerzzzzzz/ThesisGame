using System.Collections.Generic;

public class GeneticAlgorithm
{
    private int populationSize;
    public List<DNA> population;
    private DNA bestSolution;
    
    public GeneticAlgorithm(int populationSize)
    {
        this.populationSize = populationSize;
        population = new List<DNA>();
        bestSolution = null;
    }
    
    public void InitializePopulation(List<int> batch, List<float> difficulty, int distance, int monsterLevel)
    {
        for (int i = 0; i < populationSize; i++)
        {
            population.Add(new DNA(batch, difficulty, distance, monsterLevel));
        }
        bestSolution = population[0];
    }
    
    public void OrderPopulation()
    {
        population.Sort((a, b) => b.ScoreEvaluation.CompareTo(a.ScoreEvaluation));
    }
    
    public void BestIndividual(DNA dna)
    {
        if (dna.ScoreEvaluation > bestSolution.ScoreEvaluation)
        {
            bestSolution = dna;
        }
    }
    
    public float SumEvaluations()
    {
        float sum = 0;
        foreach (var individual in population)
        {
            sum += individual.ScoreEvaluation;
        }
        return sum;
    }
    
    public int SelectParent(float sumEvaluation)
    {
        System.Random rand = new ();
        
        double randomValue = rand.NextDouble() * sumEvaluation;
        float sum = 0;
        int parent = -1;
        int i = 0;
        
        while (i < population.Count && sum < randomValue)
        {
            sum += population[i].ScoreEvaluation;
            parent += 1;
            i += 1;
        }
        return parent;
    }
    
    public DNA Solve(float mutationProbability, int numberOfGenerations, List<int> batch, List<float> difficulty, int distance, int monsterLevel)
    {
        InitializePopulation(batch, difficulty, distance, monsterLevel);

        foreach (var dna in population)
        {
            dna.CalculateFitness();
        }
        OrderPopulation();

        for (int i = 0; i < numberOfGenerations; i++)
        {
            float sum = SumEvaluations();
            List<DNA> newPopulation = new ();
            for (int j = 0; j < populationSize; j += 2)
            {
                int parent1 = SelectParent(sum);
                int parent2 = SelectParent(sum);
                var children = population[parent1].Crossover(population[parent2]);
                newPopulation.Add(children[0].Mutation(mutationProbability));
                newPopulation.Add(children[1].Mutation(mutationProbability));
            }

            population = newPopulation;

            foreach (var dna in population)
            {
                dna.CalculateFitness();
            }
            OrderPopulation();
            DNA best = population[0];
            BestIndividual(best);
        }
        
        float[] result = new float[bestSolution.Chromosome.Count];
        for (int i = 0; i < bestSolution.Chromosome.Count; i++)
        {
            result[i] = bestSolution.Chromosome[i];
        }

        return bestSolution;
    }
}

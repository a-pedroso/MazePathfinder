namespace MazePathfinder.Domain.AlgorithmServices;

using System;

internal static class AlgorithmServiceFactory
{
    //private static IServiceProvider? _serviceProvider;

    //public static void Initialize(IServiceProvider serviceProvider)
    //{
    //    _serviceProvider = serviceProvider;
    //}

    internal static IAlgorithmService GetAlgorithmService(AlgorithmsEnum algorithm)
    {
        //if (_serviceProvider == null)
        //{
        //    throw new InvalidOperationException("AlgorithmServiceFactory is not initialized.");
        //}

        //return _serviceProvider.GetRequiredKeyedService<IAlgorithmService>(algorithm.ToString());

        //return algorithm switch
        //{
        //    AlgorithmsEnum.ParcelHero => _serviceProvider.GetRequiredService<ParcelHeroAlgorithmService>(),
        //    AlgorithmsEnum.BreadthFirstSearch => _serviceProvider.GetRequiredService<BreadthFirstSearchAlgorithmService>(),
        //    _ => throw new NotImplementedException()
        //};

        return algorithm switch
        {
            AlgorithmsEnum.ParcelHero => new ParcelHeroAlgorithmService(),
            AlgorithmsEnum.BreadthFirstSearch => new BreadthFirstSearchAlgorithmService(),
            _ => throw new NotImplementedException()
        };
    }
}

﻿using AutomapperVsMapster.AttributeMapping.AutoMapper;
using AutomapperVsMapster.AttributeMapping.Mapster;
using AutomapperVsMapster.CollectionMapping;
using AutomapperVsMapster.CustomPropertyMapping;
using AutomapperVsMapster.Flattened;
using AutomapperVsMapster.NestedTypeMapping;
using AutomapperVsMapster.ReverseMappingAndUnflattening;
using AutomapperVsMapster.SimpleTypeMapping;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace AutomapperVsMapster.Benchmark;

//Paramétrage du bench pour étudier les performances sur ?net 7 et .net 6 (ce dernier va servir de référence)
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net60, baseline:true)]
//Paramétrage du bench pour mesurer l'occupation mémoire
[MemoryDiagnoser(true)]
//[Config(typeof(AntiVirusFriendlyConfig))]
public class AutomapperVsMapsterBenchmark
{
    private  readonly int _size = 100;
    //Attention cheat mode le max param doit être inférieur ou égal au _size pour faciliter la présentation de la démo.
    [Params(10, 100)]
    //[Params(1, 10)] =>  On pourrait demander des benchs pour chacune de ces valeurs
    public int SizeParam { get; set; }

    private Fakker.DAO.User[] _simpleObjectSource = null!;
    private Fakker.DAO.CollectionUser[] _collectionObjectSource = null!;
    private Fakker.DAO.NestedUser[] _nestedObjectSource = null!;
    private Flattened.User[] _flattenedObjectSource = null!;
    private CustomPropertyMapping.User[] _customPropertyObjectSource = null!;
    private ReverseMappingAndUnflattening.UserDto[] _reverseMappingObjectSource = null!;
    private AttributeMapping.AutoMapper.User[] _automapperAttributeObjectSource = null!;
    private AttributeMapping.Mapster.User[] _mapsterAttributeObjectSource = null!;

    #region SimpleMapping

    //Association d'un "Before Test" avec une liste de BenchMark.
    //Le Before test (SetupDataSourceForSimpleTypeMapping) ne sera executé qu'une seule fois
    //et bien sûr avant le lancement des benchmarks AutoMapperSimpleObjectMapping, MapsterSimpleObjectMapping .. etc.
    [GlobalSetup(Targets = new[] { 
        nameof(AutoMapperSimpleObjectMapping), 
        nameof(MapsterSimpleObjectMapping),
        nameof(AutoMapperSimpleObjectMappingDuplicate),
        nameof(OldSchoolSimpleObjectMapping)})]
    public void SetupDataSourceForSimpleTypeMapping()
    {
        _simpleObjectSource = SimpleTypeMappingDataGenerator.GetSources(_size).ToArray();
    }

    //Ici commence la description des Benchmarks
    //Chacun de ces benchs va être éxecuté dans nu thread dédié, et c'est ce thread qui va être analysé, mesuré.

    /// <summary>
    /// Bench de Automapper
    /// </summary>
    /// <returns></returns>
    [Benchmark(Description = "AutoMapper_SimpleMapping_User")] //Nom qui va apparaitre dans la synthèse
    public bool AutoMapperSimpleObjectMapping()
    {
        var cheatSize = Math.Min(_size, SizeParam);
        List<Object> list = new();

        for (var i = 0; i < cheatSize; i++)
        {
            list.Add(AutoMapperSimpleTypeMapping.Map(_simpleObjectSource[i]));
        }
        return list.Count != 0; // le retour d'un paramètre est une "bonne pratique"
    }

    /// <summary>
    /// Bench de Mapster
    /// </summary>
    /// <returns></returns>
    [Benchmark(Description = "Mapster_SimpleMapping_User")]
    public bool MapsterSimpleObjectMapping()
    {
        var cheatSize = Math.Min(_size, SizeParam);
        List<Object> list = new();

        for (var i = 0; i < cheatSize; i++)
        {
            list.Add(MapsterSimpleTypeMapping.Map(_simpleObjectSource[i]));
        }
        return list.Count != 0;

    }

    /// <summary>
    /// Ce bench est présent pour mettre en évidence le non déterminisme.
    /// Les résultats de ce test doivent être proche de Mapster_SimpleMapping_User.
    /// S'ils sont éloignés, on devrait pouvoir remarquer un nombre d'erreurs très différents dans la synthèse.
    /// On tâche en effet de comparer à "nombre d'erreurs équivalentes"
    /// </summary>
    /// <returns></returns>
    [Benchmark(Description = "Mapster_SimpleMapping_User_Duplicate")]
    public bool  AutoMapperSimpleObjectMappingDuplicate()
    {
        var cheatSize = Math.Min(_size, SizeParam);
        List<Object> list = new();

        for (var i = 0; i < cheatSize; i++)
        {
            list.Add(AutoMapperSimpleTypeMapping.Map(_simpleObjectSource[i]));
        }

        return list.Count != 0;
    }

    /// <summary>
    /// Bench du OldSchool : à comparer avec les résultats obtenus dans le benchmark oldSchool
    /// </summary>
    /// <returns></returns>
    [Benchmark(Description = "OldSchool_SimpleMapping_User")]
    public bool OldSchoolSimpleObjectMapping()
    {
        List<Object> list = new ();
        var cheatSize = Math.Min(_size, SizeParam);
        for (var i = 0; i < cheatSize; i++)
        {
            list.Add(OldSchoolSimpleTypeMapping.Map(_simpleObjectSource[i]));
        }
        return list.Count != 0;
    }
    #endregion

    #region ListOrArrayMapping

    [GlobalSetup(Targets = new[] { nameof(AutoMapperListOrArrayMapping), nameof(MapsterListOrArrayMapping) })]
    public void SetupDataSourceForCollectionMapping()
    {
        _collectionObjectSource = CollectionMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "AutoMapper_ListOrArrayMapping")]
    public void AutoMapperListOrArrayMapping()
    {
        AutoMapperCollectionMapping.Map(_collectionObjectSource.ToList());
    }

    //[Benchmark(Description = "Mapster_ListOrArrayMapping")]
    public void MapsterListOrArrayMapping()
    {
        MapsterCollectionMapping.Map(_collectionObjectSource.ToList());
    }

    #endregion

    #region NestedMapping

    [GlobalSetup(Targets = new[] { nameof(AutoMapperNestedObjectMapping), nameof(MapsterNestedObjectMapping) })]
    public void SetupDataSourceForNestedTypeMapping()
    {
        _nestedObjectSource = NestedTypeMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "AutoMapper_NestedMapping")]
    public void AutoMapperNestedObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            AutoMapperNestedTypeMapping.Map(_nestedObjectSource[i]);
        }
    }

    //[Benchmark(Description = "Mapster_NestedMapping")]
    public void MapsterNestedObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            MapsterNestedTypeMapping.Map(_nestedObjectSource[i]);
        }
    }

    #endregion

    #region FlattenedMapping

    [GlobalSetup(Targets = new[] { nameof(AutoMapperFlattenedObjectMapping), nameof(MapsterFlattenedObjectMapping) })]
    public void SetupDataSourceForFlattenedMapping()
    {
        _flattenedObjectSource = FlattenedMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "AutoMapper_FlattenedMapping")]
    public void AutoMapperFlattenedObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            AutoMapperFlattenedMapping.Map(_flattenedObjectSource[i]);
        }
    }

    //[Benchmark(Description = "Mapster_FlattenedMapping")]
    public void MapsterFlattenedObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            MapsterFlattenedMapping.Map(_flattenedObjectSource[i]);
        }
    }

    #endregion

    #region CustomPropertyMapping

    [GlobalSetup(Targets = new[] { nameof(AutoMapperCustomPropertyObjectMapping), nameof(MapsterCustomPropertyObjectMapping) })]
    public void SetupDataSourceForCustomPropertyMapping()
    {
        MapsterCustomPropertyMapping.Initialize();
        _customPropertyObjectSource = CustomPropertyMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "AutoMapper_CustomPropertyMapping")]
    public void AutoMapperCustomPropertyObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            AutoMapperCustomPropertyMapping.Map(_customPropertyObjectSource[i]);
        }
    }

    //[Benchmark(Description = "Mapster_CustomPropertyMapping")]
    public void MapsterCustomPropertyObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            MapsterCustomPropertyMapping.Map(_customPropertyObjectSource[i]);
        }
    }

    #endregion

    #region CustomPropertyMapping

    [GlobalSetup(Targets = new[] { nameof(AutoMapperReverseObjectMapping), nameof(MapsterReverseObjectMapping) })]
    public void SetupDataSourceForReverseMapping()
    {
        MapsterReverseMapping.Initialize();
        _reverseMappingObjectSource = ReverseMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "AutoMapper_ReverseMapping")]
    public void AutoMapperReverseObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            AutoMapperReverseMapping.Map(_reverseMappingObjectSource[i]);
        }
    }

    //[Benchmark(Description = "Mapster_ReverseMapping")]
    public void MapsterReverseObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            MapsterReverseMapping.Map(_reverseMappingObjectSource[i]);
        }
    }

    #endregion

    #region AttributeMapping

    [GlobalSetup(Targets = new[] { nameof(AutoMapperAttributeObjectMapping) })]
    public void SetupDataSourceForAutoMapperAttributeMapping()
    {
        _automapperAttributeObjectSource = AutoMapperAttributeMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "AutoMapper_AttributeMapping")]
    public void AutoMapperAttributeObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            AutoMapperAttributeMapping.Map(_automapperAttributeObjectSource[i]);
        }
    }

    [GlobalSetup(Targets = new[] { nameof(MapsterAttributeObjectMapping) })]
    public void SetupDataSourceForMapsterAttributeMapping()
    {
        _mapsterAttributeObjectSource = MapsterAttributeMappingDataGenerator.GetSources(_size).ToArray();
    }

    //[Benchmark(Description = "Mapster_AttributeMapping")]
    public void MapsterAttributeObjectMapping()
    {
        for (var i = 0; i < _size; i++)
        {
            MapsterAttributeMapping.Map(_mapsterAttributeObjectSource[i]);
        }
    }

    #endregion
}
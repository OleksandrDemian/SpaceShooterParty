using UnityEngine;

public interface IPoolable {
    EntityType Type { get; }
    GameObject Get { get; }
}

using UnityEngine;

public interface IPoolable {
    GOType Type { get; }
    GameObject Get { get; }
}

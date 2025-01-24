using UnityEngine;

public abstract class PlayerSystem : MonoBehaviour
{
    protected Player _player;

    public void InjectPlayer(Player player)
    {
        _player = player;
    }
}
using System;
using Shared.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestCharacterModel : MonoBehaviour
{
    [SerializeField] public Transform Transform;
    [SerializeField] public Text NameText;

    public CharacterSharedModel Model;

    private void Update()
    {
        if (Model == null) return;

        var direction = new Vector3(Model.Direction.Value.X, Model.Direction.Value.Y, Model.Direction.Value.Z);
        Transform.position += direction * Time.deltaTime;
    }
}

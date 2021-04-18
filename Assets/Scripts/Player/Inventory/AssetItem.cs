using System;

using UnityEngine;
[CreateAssetMenu(menuName ="Item")]
public class AssetItem : ScriptableObject,  IItem
{
    public string Name => _name;

    public Sprite UIIcon => _uiIcon;

    public string Description => _desription;

    [SerializeField] private string _name;
    [SerializeField] private Sprite _uiIcon;
    [SerializeField] private string _desription;

}

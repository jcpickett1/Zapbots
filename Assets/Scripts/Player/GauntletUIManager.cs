using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GauntletUIManager : MonoBehaviour
{
    [SerializeField] private SpellGauntlet _myGauntlet;
    [SerializeField] private Image _activeSprite;
    [SerializeField] private Image _previousSprite;
    [SerializeField] private Image _nextSprite;

    private Sprite[] _sprites;
    private SpellData[] _spells;
    private int _lastFrameIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spells = _myGauntlet.GetSpells;
        _sprites = new Sprite[_spells.Length];
        for (int i = 0; i < _spells.Length; i++)
        {
            _sprites[i] = Resources.Load<Sprite>(_spells[i].Filename);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int thisFrameIndex = _myGauntlet.GetIndex;
        if (_lastFrameIndex != thisFrameIndex)
        {
            int previousSpellIndex = (thisFrameIndex == 0) ? 2 : thisFrameIndex - 1;
            int nextSpellIndex = (thisFrameIndex + 1) % 3;

            _activeSprite.sprite = _sprites[thisFrameIndex];
            _previousSprite.sprite = _sprites[previousSpellIndex];
            _nextSprite.sprite = _sprites[nextSpellIndex];

            _lastFrameIndex = thisFrameIndex;
        }
    }
}

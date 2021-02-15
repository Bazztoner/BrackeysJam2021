using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public Dictionary<SeedTypes, bool> unlockedSeeds;
    public Dictionary<KeyControl, SeedTypes> seedInputs;
    Queue<SeedTypes> _currentSeedCombo;
    public byte maxCombo;

    Keyboard _kb;
    Mouse _mb;

    void Start()
    {
        _kb = Keyboard.current;
        _mb = Mouse.current;

        _currentSeedCombo = new Queue<SeedTypes>();

        InitializeDictionaries();
    }

    void Update()
    {
        CheckMovement();
        CheckMouseLook();
        CheckSeedInput();
        CheckMouseInput();
    }

    void InitializeDictionaries()
    {
        unlockedSeeds = new Dictionary<SeedTypes, bool>
        {
            { SeedTypes.Base, true },
            { SeedTypes.Root, true },
            { SeedTypes.Explosive, true },
            { SeedTypes.Bouncer, true },
            { SeedTypes.Seeker, true },
            { SeedTypes.Parasite, true }
        };

        seedInputs = new Dictionary<KeyControl, SeedTypes>
        {
            {_kb.digit1Key, SeedTypes.Base },
            {_kb.digit2Key, SeedTypes.Root },
            {_kb.digit3Key, SeedTypes.Explosive },
            {_kb.digit4Key, SeedTypes.Bouncer },
            {_kb.digit5Key, SeedTypes.Seeker },
            {_kb.digit6Key, SeedTypes.Parasite }
        };
    }

    void CheckMovement()
    {

    }

    void CheckMouseLook()
    {

    }

    void CheckMouseInput()
    {
        if (_mb.leftButton.wasPressedThisFrame)
        {
            //shoot shit
            string st = "";
            foreach (var item in _currentSeedCombo)
            {
                st += item.ToString() + " ";
            }

            print("Combo: " + st);
            _currentSeedCombo = new Queue<SeedTypes>();
        }
        else if (_mb.rightButton.wasPressedThisFrame)
        {
            //delete queue
            _currentSeedCombo = new Queue<SeedTypes>();
            print("Queue was emptied");

            string st = "";
            foreach (var item in _currentSeedCombo)
            {
                st += item.ToString() + " ";
            }

            print("Combo:" + st);
        }
    }

    void CheckSeedInput()
    {
        if (_kb.digit1Key.wasPressedThisFrame)
        {
            var seed = seedInputs[_kb.digit1Key];
            if (unlockedSeeds[seed]) ManageCombo(seed);
        }
        if (_kb.digit2Key.wasPressedThisFrame)
        {
            var seed = seedInputs[_kb.digit2Key];
            if (unlockedSeeds[seed]) ManageCombo(seed);
        }
        if (_kb.digit3Key.wasPressedThisFrame)
        {
            var seed = seedInputs[_kb.digit3Key];
            if (unlockedSeeds[seed]) ManageCombo(seed);
        }
        if (_kb.digit4Key.wasPressedThisFrame)
        {
            var seed = seedInputs[_kb.digit4Key];
            if (unlockedSeeds[seed]) ManageCombo(seed);
        }
        if (_kb.digit5Key.wasPressedThisFrame)
        {
            var seed = seedInputs[_kb.digit5Key];
            if (unlockedSeeds[seed]) ManageCombo(seed);
        }
        if (_kb.digit6Key.wasPressedThisFrame)
        {
            var seed = seedInputs[_kb.digit6Key];
            if (unlockedSeeds[seed]) ManageCombo(seed);
        }
    }

    void ManageCombo(SeedTypes addTo)
    {
        if (_currentSeedCombo == null) _currentSeedCombo = new Queue<SeedTypes>();

        if (_currentSeedCombo.Count < maxCombo)
        {
            _currentSeedCombo.Enqueue(addTo);
            //add graphics to UI
        }
        else
        {
            //do gray shit with UI
        }
    }

    public void GetNewSeed(SeedTypes newSeed)
    {
        unlockedSeeds[newSeed] = true;
    }
}

public enum SeedTypes
{
    Base,
    Root,
    Explosive,
    Bouncer,
    Seeker,
    Parasite
}
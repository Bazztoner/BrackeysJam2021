using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : Entity
{
    public float movementSpeed;
    public Dictionary<SeedTypes, bool> unlockedSeeds;
    public Dictionary<KeyControl, SeedTypes> seedInputs;
    Queue<SeedTypes> _currentSeedCombo;
    public byte maxCombo;

    Keyboard _kb;
    Mouse _mb;
    Vector2 _mousePos;
    Rigidbody2D _rb;
    Camera _cam;
    Vector3 _camOffset;
    Quaternion _camRotOffset;
    Vector3 _dumpSpeed;

    void Start()
    {
        _kb = Keyboard.current;
        _mb = Mouse.current;
        _rb = GetComponent<Rigidbody2D>();

        _currentSeedCombo = new Queue<SeedTypes>();

        InitializeDictionaries();

        _cam = Camera.main;
        //Normally 0, 2.46, -10
        _camOffset = _cam.transform.position;
        _camRotOffset = _cam.transform.rotation;

        Cursor.lockState = CursorLockMode.Confined;

    }

    void Update()
    {
        CheckMouseLook();
        CheckSeedInput();
        CheckMouseInput();
    }

    void FixedUpdate()
    {
        CheckMovement();
    }

    void LateUpdate()
    {
        _cam.transform.position = transform.position + _camOffset;
    }

    void InitializeDictionaries()
    {
        //Remember to put false in everything except Base for the final game
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
        Vector2 movementVector = Vector2.zero;

        if (_kb.wKey.isPressed)
        {
            movementVector += new Vector2(transform.up.x * movementSpeed * Time.fixedDeltaTime, transform.up.y * movementSpeed * Time.fixedDeltaTime);
        }

        if (_kb.sKey.isPressed)
        {
            movementVector -= new Vector2(transform.up.x * movementSpeed * Time.fixedDeltaTime, transform.up.y * movementSpeed * Time.fixedDeltaTime);
        }

        if (_kb.aKey.isPressed)
        {
            movementVector -= new Vector2(transform.right.x * movementSpeed * Time.fixedDeltaTime, transform.right.y * movementSpeed * Time.fixedDeltaTime);
        }

        if (_kb.dKey.isPressed)
        {
            movementVector += new Vector2(transform.right.x * movementSpeed * Time.fixedDeltaTime, transform.right.y * movementSpeed * Time.fixedDeltaTime);
        }

        _rb.MovePosition(_rb.position + movementVector);
    }

    void CheckMouseLook()
    {
        _mousePos = _mb.position.ReadValue();

        var targetDir = Camera.main.ScreenToWorldPoint(_mousePos) - transform.position;
        var angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void CheckMouseInput()
    {
        if (_mb.leftButton.wasPressedThisFrame && _currentSeedCombo != null && _currentSeedCombo.Any())
        {
            //shoot shit
            string st = "";
            foreach (var item in _currentSeedCombo)
            {
                st += item.ToString() + " ";
            }

            print("Combo: " + st);
            GetComponent<ComboSystem>().DefineCombo(_currentSeedCombo);
            _currentSeedCombo = new Queue<SeedTypes>();
        }
        else if (_mb.rightButton.wasPressedThisFrame)
        {
            //delete queue
            _currentSeedCombo = new Queue<SeedTypes>();
            print("Combo was emptied");
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

    public override void TakeDamage(float dmg)
    {
        //implement later
    }
}

public enum SeedTypes
{
    Base = 0,
    Root = 1,
    Explosive = 2,
    Bouncer = 3,
    Seeker = 4,
    Parasite = 5
}
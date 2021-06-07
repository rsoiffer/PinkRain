using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PinkRain.Component
{
    public class CharacterPicker : MonoBehaviour
    {
        private static readonly IReadOnlyDictionary<KeyCode, int> CharacterKeys = new Dictionary<KeyCode, int>
        {
            [KeyCode.Alpha1] = 0,
            [KeyCode.Alpha2] = 1,
            [KeyCode.Alpha3] = 2,
            [KeyCode.Alpha4] = 3,
            [KeyCode.Alpha5] = 4
        };

        [SerializeField] private GameObject[] characterPrefabs = Array.Empty<GameObject>();
        [SerializeField] private Slider? healthSlider;

        private readonly List<GameObject> characters = new List<GameObject>();

        public GameObject? ActiveCharacter { get; private set; }

        private void Awake()
        {
            foreach (var prefab in characterPrefabs)
            {
                var character = Instantiate(prefab, transform.parent);
                character.GetComponent<Health>().uiSlider = healthSlider;
                characters.Add(character);
            }

            Activate(characters.First());
        }

        private void Update()
        {
            if (CharacterKeyDown() is { } index && index < characters.Count)
            {
                Activate(characters[index]);
            }

            if (ActiveCharacter)
            {
                transform.position = ActiveCharacter!.transform.position;
            }
        }

        private void Activate(GameObject character)
        {
            ActiveCharacter = character;
            ActiveCharacter.transform.position = transform.position;
            foreach (var otherCharacter in characters)
            {
                otherCharacter.SetActive(otherCharacter == ActiveCharacter);
            }
        }

        private static int? CharacterKeyDown()
        {
            var indices =
                from item in CharacterKeys
                where Input.GetKeyDown(item.Key)
                select (int?) item.Value;

            return indices.FirstOrDefault();
        }
    }
}
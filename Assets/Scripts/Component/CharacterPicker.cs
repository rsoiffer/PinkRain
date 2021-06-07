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
        private int index;

        public GameObject Active => characters[index];

        private void Awake()
        {
            foreach (var prefab in characterPrefabs)
            {
                var character = Instantiate(prefab, transform.parent);
                character.GetComponent<Health>().uiSlider = healthSlider;
                characters.Add(character);
            }

            Activate(0);
        }

        private void Update()
        {
            if (CharacterKeyDown() is { } index && index < characters.Count && characters[index])
            {
                Activate(index);
            }
            else if (Active)
            {
                transform.position = Active.transform.position;
            }
            else if (this.index < characters.Count - 1)
            {
                Activate(this.index + 1);
            }
        }

        private void Activate(int index)
        {
            var liveCharacters = characters
                .Select((character, i) => (character, i))
                .Where(item => item.character);

            foreach (var (character, i) in liveCharacters)
            {
                character.SetActive(index == i);
            }

            this.index = index;
            Active.transform.position = transform.position;
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
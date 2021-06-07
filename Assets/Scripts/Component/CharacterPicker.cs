using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PinkRain.Component
{
    public class CharacterPicker : MonoBehaviour
    {
        [SerializeField] private GameObject[] characterPrefabs = Array.Empty<GameObject>();
        [SerializeField] private Slider? healthSlider;

        public GameObject? ActiveCharacter { get; private set; }

        private readonly List<GameObject> characters = new List<GameObject>();

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
    }
}